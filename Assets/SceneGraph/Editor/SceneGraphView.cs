using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<SceneGraphView, UxmlTraits>
    {
    }

    public Action<BaseNodeView> OnNodeSelected;
    public SceneGraphData SceneGraphData { get; private set; }

    public SceneGraphView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/SceneGraph/Editor/SceneGraph.uss");
        styleSheets.Add(styleSheet);
    }

    public void PopulateView(SceneGraphData sceneGraphData)
    {
        SceneGraphData = sceneGraphData;

        graphViewChanged -= OnGraphViewChange;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChange;

        var nodeViews = new List<BaseNodeView>();
        
        foreach (var node in SceneGraphData.Nodes)
        {
            BaseNodeView nodeView = node switch
            {
                SceneNodeData sceneNode => new SceneNodeView(sceneNode),
                SpawnNodeData spawnNode => new SpawnNodeView(spawnNode),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            nodeView.OnNodeSelected = OnNodeSelected;
            
            nodeViews.Add(nodeView);
            AddElement(nodeView);
        }

        foreach (var nodeView in nodeViews)
        {
            foreach (var (key, port) in nodeView.Inputs)
            {
                var data = (SpawnData)port.userData;
                
                if(!data.SetUp)
                    continue;
                
                if(data.Port != "input")
                    continue;
                
                if(port.connected)
                {
                    nodeView.RemoveOutput(data.Id);
                    continue;
                }

                var otherNode = nodeViews.FirstOrDefault(n => n.viewDataKey == data.SceneDestination);
                
                if(otherNode == null)
                    continue;

                var otherPort =
                    otherNode.Outputs.FirstOrDefault(d => ((SpawnData) d.Value.userData).Id == data.DoorDestination).Value;
                
                if(otherPort == null)
                    continue;
                
                var edge = port.ConnectTo(otherPort);
                
                nodeView.RemoveOutput(data.Id);
                AddElement(edge);
            }
            
            foreach (var (key, port) in nodeView.Outputs)
            {
                var data = (SpawnData)port.userData;
                
                if(!data.SetUp)
                    continue;
                
                if(data.Port != "output")
                    continue;
                
                if(port.connected)
                {
                    nodeView.RemoveInput(data.Id);
                    continue;
                }
                
                var otherNode = nodeViews.FirstOrDefault(n => n.viewDataKey == data.SceneDestination);
                
                if(otherNode == null)
                    continue;

                var otherPort =
                    otherNode.Inputs.FirstOrDefault(d => ((SpawnData) d.Value.userData).Id == data.DoorDestination).Value;
                
                if(otherPort == null)
                    continue;
                
                var edge = port.ConnectTo(otherPort);
                nodeView.RemoveInput(data.Id);
                AddElement(edge);
            }

            FrameAll();
        }
    }

    private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var element in graphViewChange.elementsToRemove)
            {
                if (element is BaseNodeView nodeView)
                {
                    SceneGraphData.DeleteNode(nodeView.NodeData);
                }
                
                if (element is Edge edge)
                {
                    var input = edge.input;
                    var output = edge.output;

                    var spawnInputData = (SpawnData)input.userData;
                    var spawnOutputData = (SpawnData)output.userData;
                    
                    spawnInputData.SceneDestination = "";
                    spawnInputData.DoorDestination = "";
                    spawnInputData.Port = "";
                    spawnInputData.SetUp = false;
                    ((BaseNodeView) input.node).AddOutput(spawnInputData.Id);

                    spawnOutputData.SceneDestination = "";
                    spawnOutputData.DoorDestination = "";
                    spawnInputData.Port = "";
                    spawnOutputData.SetUp = false;
                    ((BaseNodeView) output.node).AddInput(spawnOutputData.Id);
                }
            }
            
        }
        
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var input = edge.input;
                var output = edge.output;
                
                var spawnInputData = (SpawnData)input.userData;
                var spawnOutputData = (SpawnData)output.userData;
                
                spawnInputData.SceneDestination = ((BaseNodeView) output.node).NodeData.Id;
                spawnInputData.DoorDestination = spawnOutputData.Id;
                spawnInputData.Port = "input";
                spawnInputData.SetUp = true;
                ((BaseNodeView) input.node).RemoveOutput(spawnInputData.Id);

                spawnOutputData.SceneDestination = ((BaseNodeView) input.node).NodeData.Id;
                spawnOutputData.DoorDestination = spawnInputData.Id;
                spawnOutputData.Port = "output";
                spawnOutputData.SetUp = true;
                ((BaseNodeView) output.node).RemoveInput(spawnOutputData.Id);
            }
            
        }
        return graphViewChange;
    }

    private void CreateSceneNode(Vector2 eventInfoLocalMousePosition)
    {
        var path = EditorUtility.OpenFilePanel("Choose prefab", "Assets/Prefabs/Rooms", "prefab");

        if (path != null)
        {
            string[] separatedPath = path.Split(new[] {"Assets"}, StringSplitOptions.None);

            var prefab = AssetDatabase.LoadAssetAtPath<DungeonRoomController>("Assets" + separatedPath[1]);
            var node = SceneGraphData.AddSceneNode(prefab);
            node.Position = viewTransform.matrix.inverse.MultiplyPoint(eventInfoLocalMousePosition);
            
            var nodeView = new SceneNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
            
            ClearSelection();
            AddToSelection(nodeView);
        }
    }

    private void CreateSpawnNode(Vector2 eventInfoLocalMousePosition)
    {
        var node = SceneGraphData.AddSpawnNode();
        node.Position = viewTransform.matrix.inverse.MultiplyPoint(eventInfoLocalMousePosition);
            
        var nodeView = new SpawnNodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
            
        ClearSelection();
        AddToSelection(nodeView);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Spawn", (a) => CreateSpawnNode(a.eventInfo.localMousePosition));
        evt.menu.AppendAction("Scene", (a) => CreateSceneNode(a.eventInfo.localMousePosition));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }
}
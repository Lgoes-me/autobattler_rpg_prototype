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
            var nodeView = node.ToNodeView();
            
            nodeView.OnNodeSelected = OnNodeSelected;
            
            nodeViews.Add(nodeView);
            AddElement(nodeView);
        }

        foreach (var nodeView in nodeViews)
        {
            foreach (var (key, port) in nodeView.Inputs)
            {
                var data = (DoorData)port.userData;
                
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
                    otherNode.Outputs.FirstOrDefault(d => ((DoorData) d.Value.userData).Id == data.DoorDestination).Value;
                
                if(otherPort == null)
                    continue;
                
                var edge = port.ConnectTo(otherPort);
                nodeView.RemoveOutput(data.Id);
                AddElement(edge);
            }
            
            foreach (var (key, port) in nodeView.Outputs)
            {
                var data = (DoorData)port.userData;
                
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
                    otherNode.Inputs.FirstOrDefault(d => ((DoorData) d.Value.userData).Id == data.DoorDestination).Value;
                
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

                    var spawnInputData = (DoorData)input.userData;
                    var spawnOutputData = (DoorData)output.userData;
                    
                    spawnInputData.SceneDestination = "";
                    spawnInputData.DoorDestination = "";
                    spawnInputData.Port = "";
                    spawnInputData.SetUp = false;
                    ((BaseNodeView) input.node).AddOutput(spawnInputData.Id);
                    EditorUtility.SetDirty(((BaseNodeView) input.node).NodeData);

                    spawnOutputData.SceneDestination = "";
                    spawnOutputData.DoorDestination = "";
                    spawnInputData.Port = "";
                    spawnOutputData.SetUp = false;
                    ((BaseNodeView) output.node).AddInput(spawnOutputData.Id);
                    EditorUtility.SetDirty(((BaseNodeView) output.node).NodeData);

                    AssetDatabase.SaveAssets();
                }
            }
            
        }
        
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var input = edge.input;
                var output = edge.output;
                
                var spawnInputData = (DoorData)input.userData;
                var spawnOutputData = (DoorData)output.userData;
                
                spawnInputData.SceneDestination = ((BaseNodeView) output.node).NodeData.Id;
                spawnInputData.DoorDestination = spawnOutputData.Id;
                spawnInputData.Port = "input";
                spawnInputData.SetUp = true;
                ((BaseNodeView) input.node).RemoveOutput(spawnInputData.Id);
                EditorUtility.SetDirty(((BaseNodeView) input.node).NodeData);

                spawnOutputData.SceneDestination = ((BaseNodeView) input.node).NodeData.Id;
                spawnOutputData.DoorDestination = spawnInputData.Id;
                spawnOutputData.Port = "output";
                spawnOutputData.SetUp = true;
                ((BaseNodeView) output.node).RemoveInput(spawnOutputData.Id);
                EditorUtility.SetDirty(((BaseNodeView) output.node).NodeData);
                
                AssetDatabase.SaveAssets();
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
            
            var id = Guid.NewGuid().ToString();
            var prefab = AssetDatabase.LoadAssetAtPath<RoomController>("Assets" + separatedPath[1]);
            
            var nodeParams = new SceneNodeDataParams(id, prefab);
            
            CreateNode<SceneNodeData>(eventInfoLocalMousePosition, nodeParams);
        }
    }

    private void CreateSpawnNode(Vector2 eventInfoLocalMousePosition)
    {
        var id = Guid.NewGuid().ToString();
        var nodeParams = new SpawnNodeDataParams(id);
        
        CreateNode<SpawnNodeData>(eventInfoLocalMousePosition, nodeParams);
    }

    private void CreateDungeonNode(Vector2 eventInfoLocalMousePosition)
    {
        var id = Guid.NewGuid().ToString();
        var nodeParams = new DungeonNodeDataParams(id);
        
        CreateNode<DungeonNodeData>(eventInfoLocalMousePosition, nodeParams);
    }

    private void CreateNode<T>(Vector2 eventInfoLocalMousePosition, NodeDataParams nodeParams) where T : BaseNodeData
    {
        var node = SceneGraphData.AddNode<T>();
        
        node.Init(nodeParams);
        node.SetPosition(viewTransform.matrix.inverse.MultiplyPoint(eventInfoLocalMousePosition));
            
        var nodeView = node.ToNodeView();
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
            
        ClearSelection();
        AddToSelection(nodeView);
        
        EditorUtility.SetDirty(node);
        AssetDatabase.SaveAssets();
    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Spawn", (a) => CreateSpawnNode(a.eventInfo.localMousePosition));
        evt.menu.AppendAction("Scene", (a) => CreateSceneNode(a.eventInfo.localMousePosition));
        evt.menu.AppendAction("Dungeon", (a) => CreateDungeonNode(a.eventInfo.localMousePosition));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }
}
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

        foreach (var node in SceneGraphData.Nodes)
        {
            BaseNodeView nodeView = node switch
            {
                SceneNodeData sceneNode => new SceneNodeView(sceneNode),
                SpawnNodeData spawnNode => new SpawnNodeView(spawnNode),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
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
                
                    if (input.userData is SpawnData spawnInputData && output.userData is SpawnData spawnOutputData)
                    {
                        spawnInputData.SceneDestination = "";
                        spawnInputData.DoorDestination = "";
                        spawnInputData.SetUp = false;
                        ((BaseNodeView) input.node).AddOutput(spawnInputData.Id);

                        spawnOutputData.SceneDestination = "";
                        spawnOutputData.DoorDestination = "";
                        spawnOutputData.SetUp = false;
                        ((BaseNodeView) output.node).AddInput(spawnOutputData.Id);
                    }
                }
            }
            
        }
        
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var input = edge.input;
                var output = edge.output;
                
                if (input.userData is SpawnData spawnInputData && output.userData is SpawnData spawnOutputData)
                {
                    spawnInputData.SceneDestination = ((BaseNodeView) output.node).NodeData.Id;
                    spawnInputData.DoorDestination = spawnOutputData.Id;
                    spawnInputData.SetUp = true;
                    ((BaseNodeView) input.node).RemoveOutput(spawnInputData.Id);

                    spawnOutputData.SceneDestination = ((BaseNodeView) input.node).NodeData.Id;
                    spawnOutputData.DoorDestination = spawnInputData.Id;
                    spawnOutputData.SetUp = true;
                    ((BaseNodeView) output.node).RemoveInput(spawnOutputData.Id);
                }
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
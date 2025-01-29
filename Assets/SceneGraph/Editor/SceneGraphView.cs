using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class SceneGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<SceneGraphView, UxmlTraits>
    {
    }

    public Action<SceneNodeView> OnNodeSelected;
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
            var nodeView = new SceneNodeView(node);
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
                if (element is SceneNodeView nodeView)
                {
                    SceneGraphData.DeleteNode(nodeView.SceneNodeData);
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
                        ((SceneNodeView) input.node).AddOutput(spawnInputData.Id);

                        spawnOutputData.SceneDestination = "";
                        spawnOutputData.DoorDestination = "";
                        spawnOutputData.SetUp = false;
                        ((SceneNodeView) output.node).AddInput(spawnOutputData.Id);
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
                    spawnInputData.SceneDestination = ((SceneNodeView) output.node).SceneNodeData.Id;
                    spawnInputData.DoorDestination = spawnOutputData.Id;
                    spawnInputData.SetUp = true;
                    ((SceneNodeView) input.node).RemoveOutput(spawnInputData.Id);

                    spawnOutputData.SceneDestination = ((SceneNodeView) input.node).SceneNodeData.Id;
                    spawnOutputData.DoorDestination = spawnInputData.Id;
                    spawnOutputData.SetUp = true;
                    ((SceneNodeView) output.node).RemoveInput(spawnOutputData.Id);
                }
            }
            
        }
        return graphViewChange;
    }

    private void CreateEmptyNode()
    {
        var path = EditorUtility.OpenFilePanel("Choose prefab", "Assets/Prefabs/Rooms", "prefab");

        if (path != null)
        {
            string[] separatedPath = path.Split(new[] {"Assets"}, StringSplitOptions.None);

            var prefab = AssetDatabase.LoadAssetAtPath<DungeonRoomController>("Assets" + separatedPath[1]);
            var node = SceneGraphData.AddSceneNode(prefab);

            var nodeView = new SceneNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
            AddToSelection(nodeView);
        }
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Create Scene Node", (a) => CreateEmptyNode());
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }
    
}
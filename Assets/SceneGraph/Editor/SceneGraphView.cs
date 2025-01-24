using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class SceneGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<SceneGraphView, UxmlTraits> { }
    
    private SceneGraphData SceneGraphData { get; set; }
    
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

    public void DeleteNodeView(SceneNodeView sceneNodeView)
    {
        SceneGraphData.DeleteNode(sceneNodeView.SceneNodeData);
        RemoveElement(sceneNodeView);
    }
    
    public void PopulateView(SceneGraphData sceneGraphData)
    {
        SceneGraphData = sceneGraphData;
        
        DeleteElements(graphElements);
        
        foreach (var (id, node) in SceneGraphData?.Nodes ?? new Dictionary<string, SceneNodeData>())
        {
            CreateNodeView(node);
        }
    }

    private void CreateEmptyNode()
    {
        var path = EditorUtility.OpenFilePanel("Choose prefab", "Assets/Prefabs/Rooms", "prefab");

        if (path != null)
        {
            string[] separatedPath = path.Split(new[] { "Assets" }, StringSplitOptions.None);
            
            var prefab = AssetDatabase.LoadAssetAtPath<DungeonRoomController>("Assets" + separatedPath[1]);
            var sceneNode = SceneGraphData.AddSceneNode(prefab);
            CreateNodeView(sceneNode);
        }
    }
    
    private void CreateNodeView(SceneNodeData node)
    {
        var nodeView = new SceneNodeView(this, node);
        AddElement(nodeView);
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
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

    public void PopulateView(SceneGraphData sceneGraphData)
    {
        SceneGraphData = sceneGraphData;
        
        DeleteElements(graphElements);
        
        foreach (var (id, node) in SceneGraphData.Nodes)
        {
            CreateNodeView(node);
        }
    }

    private void CreateEmptyNode()
    {
        var sceneNode = SceneGraphData.CreateSceneNode();
        CreateNodeView(sceneNode);
    }
    
    private void CreateNodeView(SceneNodeData node)
    {
        var nodeView = new SceneNodeView(node);
        AddElement(nodeView);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Create Scene Node", (a) => CreateEmptyNode());
    }
}
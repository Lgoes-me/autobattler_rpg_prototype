using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class SceneGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<SceneGraphView, UxmlTraits> { }
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
}
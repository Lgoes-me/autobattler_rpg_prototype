using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGraph : EditorWindow
{
    private SceneGraphView SceneGraphView { get; set; }
    private InspectorView InspectorView { get; set; }

    [MenuItem("SceneGraph/Editor")]
    public static void OpenWindow()
    {
        SceneGraph wnd = GetWindow<SceneGraph>(typeof(SceneGraph), typeof(SceneView));
        wnd.titleContent = new GUIContent("SceneGraph");
    }

    public void CreateGUI()
    {
        var root = rootVisualElement;

        var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/SceneGraph/Editor/SceneGraph.uxml");
        visualTreeAsset.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/SceneGraph/Editor/SceneGraph.uss");
        root.styleSheets.Add(styleSheet);

        SceneGraphView = root.Q<SceneGraphView>();
        InspectorView = root.Q<InspectorView>();

        var asset = AssetDatabase.LoadAssetAtPath<SceneGraphData>("Assets/SceneGraph/SceneGraph.asset");

        SceneGraphView.OnNodeSelected = OnNodeSelectionChanged;
        SceneGraphView.focusable = true;
        SceneGraphView.PopulateView(asset);
    }

    private void OnSelectionChange()
    {
        if (Selection.activeObject is SceneGraphData sceneGraphData && SceneGraphView.SceneGraphData != sceneGraphData)
        {
            SceneGraphView.PopulateView(sceneGraphData);
        }
    }

    private void OnNodeSelectionChanged(BaseNodeView nodeView)
    {
        InspectorView.UpdateSelection(nodeView);
    }
    
}
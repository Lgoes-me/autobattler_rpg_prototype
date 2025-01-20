using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGraph : EditorWindow
{
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
    }
}
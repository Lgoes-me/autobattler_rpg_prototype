using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapEditor : EditorWindow
{
    private MapEditorView MapEditorView { get; set; }
    
    [MenuItem("GameTools/MapEditor")]
    public static void OpenWindow()
    {
        var wnd = GetWindow<MapEditor>(typeof(MapEditor), typeof(SceneView));
        wnd.titleContent = new GUIContent("MapEditor");
    }

    public void CreateGUI()
    {
        var root = rootVisualElement;

        var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/MapEditor/Editor/MapEditor.uxml");
        visualTreeAsset.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/MapEditor/Editor/MapEditor.uss");
        root.styleSheets.Add(styleSheet);

        MapEditorView = root.Q<MapEditorView>();

        var asset = AssetDatabase.LoadAssetAtPath<MapData>("Assets/MapEditor/Map.asset");

        MapEditorView.OnNodeSelected = OnNodeSelectionChanged;
        MapEditorView.focusable = true;
        MapEditorView.PopulateView(asset);
    }

    private void OnSelectionChange()
    {
        if (Selection.activeObject is MapData mapData && MapEditorView.MapData != mapData)
        {
            MapEditorView.PopulateView(mapData);
        }
    }

    private void OnNodeSelectionChanged(BaseNodeView nodeView)
    {
        if(nodeView == null)
            return;
        
        Selection.SetActiveObjectWithContext(nodeView.NodeData, this);
    }
}
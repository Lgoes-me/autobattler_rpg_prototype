using UnityEditor;
using UnityEngine.UIElements;

public class SceneNodeView : BaseNodeView
{
    private SceneNodeData SceneNodeData { get; }
    
    public SceneNodeView(SceneNodeData sceneNodeData) : base(sceneNodeData)
    {
        SceneNodeData = sceneNodeData;
        
        var preview = new Image();

        preview.image = AssetPreview.GetAssetPreview(SceneNodeData.RoomPrefab.gameObject) ??
                        AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(SceneNodeData.RoomPrefab.gameObject));

        mainContainer.Add(preview);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Open Prefab", _ => OpenPrefab());
        base.BuildContextualMenu(evt);
    }

    private void OpenPrefab()
    {
        AssetDatabase.OpenAsset(SceneNodeData.RoomPrefab);
    }
}
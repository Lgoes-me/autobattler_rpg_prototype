using UnityEditor;
using UnityEngine.UIElements;

public class CutsceneNodeView : BaseNodeView
{
    private CutsceneNodeData CutsceneNodeData { get; }

    public CutsceneNodeView(CutsceneNodeData sceneNodeData) : base(sceneNodeData)
    {
        CutsceneNodeData = sceneNodeData;
        
        var preview = new Image();

        preview.image = AssetPreview.GetAssetPreview(CutsceneNodeData.CutsceneRoomPrefab.gameObject) ??
                        AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(CutsceneNodeData.CutsceneRoomPrefab.gameObject));

        mainContainer.Add(preview);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Open Prefab", _ => OpenPrefab());
        base.BuildContextualMenu(evt);
    }

    private void OpenPrefab()
    {
        AssetDatabase.OpenAsset(CutsceneNodeData.CutsceneRoomPrefab);
    }
}
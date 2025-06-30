using UnityEditor;
using UnityEngine.UIElements;

public class CutsceneNodeView : BaseNodeView
{
    public CutsceneNodeView(CutsceneNodeData sceneNodeData) : base(sceneNodeData)
    {
        var preview = new Image();

        preview.image = AssetPreview.GetAssetPreview(sceneNodeData.CutsceneRoomPrefab.gameObject) ??
                        AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(sceneNodeData.CutsceneRoomPrefab.gameObject));

        mainContainer.Add(preview);
    }
}
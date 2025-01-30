using UnityEditor;
using UnityEngine.UIElements;

public class SceneNodeView : BaseNodeView
{
    public SceneNodeView(SceneNodeData sceneNodeData) : base(sceneNodeData)
    {
        var preview = new Image();
        preview.image = AssetPreview.GetAssetPreview(sceneNodeData.RoomPrefab);
        
        mainContainer.Add(preview);
    }
}
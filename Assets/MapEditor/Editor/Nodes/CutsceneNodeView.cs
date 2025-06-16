using UnityEngine;
using UnityEngine.UIElements;

public class CutsceneNodeView : BaseNodeView
{
    public CutsceneNodeView(CutsceneNodeData sceneNodeData) : base(sceneNodeData)
    {
        var preview = new Image();

        var prefab = Object.Instantiate(sceneNodeData.CutsceneRoomPrefab);
        var camera = prefab.PreviewCamera;
        
        Rect rect = new Rect(0, 0, camera.pixelWidth, camera.pixelHeight);
        RenderTexture renderTexture = new RenderTexture(150, 100, 24);
        Texture2D screenShot = new Texture2D(150, 100, TextureFormat.RGBA32, false);

        RenderTexture.active = renderTexture;
        
        camera.targetTexture = renderTexture;
        camera.scene = sceneNodeData.CutsceneRoomPrefab.gameObject.scene;
        
        camera.Render();
        
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        preview.image = screenShot;
        
        RenderTexture.active = null;
        camera.targetTexture = null;
        
        Object.DestroyImmediate(prefab.gameObject);
        Object.DestroyImmediate(renderTexture);

        mainContainer.Add(preview);
    }
}
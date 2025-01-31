using UnityEngine;
using UnityEngine.UIElements;

public class SceneNodeView : BaseNodeView
{
    public SceneNodeView(SceneNodeData sceneNodeData) : base(sceneNodeData)
    {
        var preview = new Image();

        var prefab = Object.Instantiate(sceneNodeData.RoomPrefab);
        var camera = prefab.CameraTeste;
        
        Rect rect = new Rect(0, 0, camera.pixelWidth, camera.pixelHeight);
        RenderTexture renderTexture = new RenderTexture(150, 100, 24);
        Texture2D screenShot = new Texture2D(150, 100, TextureFormat.RGBA32, false);

        RenderTexture.active = renderTexture;
        
        camera.targetTexture = renderTexture;
        camera.scene = sceneNodeData.RoomPrefab.gameObject.scene;
        
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
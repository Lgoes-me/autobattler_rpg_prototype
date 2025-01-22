using UnityEditor.Experimental.GraphView;

public class SceneNodeView : Node
{
    private SceneNodeData SceneNodeData { get; set; }
    
    public SceneNodeView(SceneNodeData sceneNodeData)
    {
        SceneNodeData = sceneNodeData;
        title = sceneNodeData.RoomPrefab?.gameObject.name ?? "Scene";
    }
}
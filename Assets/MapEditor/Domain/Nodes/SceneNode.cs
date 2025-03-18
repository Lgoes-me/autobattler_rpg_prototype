using System.Collections.Generic;

public class SceneNode : BaseSceneNode
{
    public RoomController RoomPrefab { get; protected set; }
    
    protected SceneNode() : base()
    {
        RoomPrefab = null;
    }
    
    public SceneNode(string name, string id, List<DoorData> doors, RoomController roomPrefab) : base(name, id, doors)
    {
        RoomPrefab = roomPrefab;
    }
}
using System.Collections.Generic;

public class SceneNodeData : BaseNodeData
{
    public DungeonRoomController RoomPrefab { get; private set; }
    
    public void Init(string id, DungeonRoomController roomPrefab)
    {
        Id = id;
        Name = name = roomPrefab.name;
        RoomPrefab = roomPrefab;
        Doors = new List<SpawnData>();
        
        foreach (var spawn in RoomPrefab.Doors)
        {
            var door = new SpawnData
            {
                Name = spawn.gameObject.name,
                Id = spawn.Id
            };

            Doors.Add(door);
        } 
    }
}
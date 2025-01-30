public class SceneNodeData : BaseNodeData
{
    public DungeonRoomController RoomPrefab { get; private set; }
    
    public void Init(string id, DungeonRoomController roomPrefab)
    {
        RoomPrefab = roomPrefab;
        base.Init(id, roomPrefab.name);
    }

    protected override void GenerateDoors()
    {
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
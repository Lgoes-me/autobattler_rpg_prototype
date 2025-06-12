using System;
using System.Collections.Generic;

public class DungeonNode : BaseSceneNode
{
    private DungeonCreator Dungeon { get; }
    
    public DungeonNode(
        string id,
        DoorData entrance,
        DoorData exit,
        List<DungeonRoomData> availableRooms) : base(id)
    {
        Dungeon = new DungeonCreator(id, entrance, exit, availableRooms);
    }

    public void Clear()
    {
        Dungeon.Clear();
    }

    public override void DoTransition(Map map, Spawn spawn, Action<SceneData, Spawn> callback)
    {
        if (!Dungeon.Generated)
        {
            Dungeon.GenerateDungeon();
        }

        if (Dungeon.GetRoom.TryGetValue(spawn.NodeId, out var room))
        {
            callback(ToSceneData(room), spawn);
            return;
        }

        var nextContext = map.AllNodesById[spawn.NodeId];
        nextContext.DoTransition(map, spawn, callback);
    }

    private SceneData ToSceneData(DungeonRoom dungeonRoom)
    {
        return new SceneData(dungeonRoom.Id,
            dungeonRoom.RoomPrefab,
            dungeonRoom.Doors,
            dungeonRoom.Music,
            dungeonRoom.CombatEncounters,
            dungeonRoom.PostProcessProfile);
    }

    public override bool IsOpen(Map map, Spawn spawn)
    {
        if (Dungeon.GetRoom.TryGetValue(spawn.NodeId, out var room))
        {
            return true;
        }

        var nextContext = map.AllNodesById[spawn.NodeId];
        return nextContext.IsOpen(map, spawn);
    }
}
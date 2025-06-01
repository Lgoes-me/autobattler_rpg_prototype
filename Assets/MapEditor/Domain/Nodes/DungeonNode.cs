using System;
using System.Collections.Generic;

public class DungeonNode : BaseSceneNode
{
    private Dungeon<DungeonRoomNode> Dungeon { get; }

    public DungeonNode(string id, List<Transition> doors, List<DungeonRoomNode> availableRooms, 
        int maximumDoors, int minimumDeepness, int maximumDeepness) : base(id, doors)
    {
        Dungeon = new Dungeon<DungeonRoomNode>(Doors[0], availableRooms, maximumDoors, minimumDeepness, maximumDeepness);
    }

    public void DoTransition(Spawn spawn, Action<SceneNode, Spawn> transition)
    {
        if (!Dungeon.Generated)
        {
            Dungeon.GenerateDungeon();
            var firstRoom = Dungeon.Rooms.Data;
            //var firstDoor = Dungeon.Rooms.Data.Doors[0].Id;

            //transition(firstRoom, new SpawnDomain(firstDoor, firstRoom.Id));
            return;
        }

        transition(Dungeon.GetRoom[spawn.NodeId], spawn);
    }

    public void Clear()
    {
        Dungeon.Clear();
    }

    public override void DoTransition(Map map, Spawn spawn, Action<SceneNode, Spawn> callback)
    {
        /*case DungeonNode dungeonNode:
            await SceneManager.LoadNewRoom();
            dungeonNode.DoTransition(spawn, SceneManager.EnterRoom);
            break;*/
    }
    
    public override bool IsOpen()
    {
        return true;
    }
}
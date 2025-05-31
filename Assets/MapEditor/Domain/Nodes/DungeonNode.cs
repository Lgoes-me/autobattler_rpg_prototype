using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DungeonNode : BaseSceneNode
{
    private Dungeon<DungeonRoomNode> Dungeon { get; }

    public DungeonNode(string name, string id, List<SpawnDomain> doors, List<DungeonRoomNode> availableRooms, 
        int maximumDoors, int minimumDeepness, int maximumDeepness) : base(name, id, doors)
    {
        Dungeon = new Dungeon<DungeonRoomNode>(Doors[0], availableRooms, maximumDoors, minimumDeepness, maximumDeepness);
    }

    public void DoTransition(SpawnDomain spawn, Action<SceneNode, SpawnDomain> transition)
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

    public override void DoTransition(SpawnDomain spawn, Map map)
    {
        /*case DungeonNode dungeonNode:
            await SceneManager.LoadNewRoom();
            dungeonNode.DoTransition(spawn, SceneManager.EnterRoom);
            break;*/
    }
}
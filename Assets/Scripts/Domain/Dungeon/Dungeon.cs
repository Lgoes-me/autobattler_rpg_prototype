using System;
using System.Linq;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public DungeonRoom BaseRoom;

    public int HowDeep = 4;

    public int MaxDoors = 4;
    
    public Tree<DungeonRoom> Rooms { get; set; }

    public void Start()
    {
        var dungeonEntrance = InstantiateRoom(0, null );
        dungeonEntrance.SetAsEntrance();
        
        Rooms = CreateRoomWithChildren(dungeonEntrance, 0);
        
        while (Rooms.Any(r => !r.Data.Collapsed))
        {
            PruneTree();
            var nextRoom = Rooms.Where(r => !r.Data.Collapsed).OrderBy(x => Guid.NewGuid()).First();
            nextRoom.Data.SetAsRoom();
        }
        
        var bossRoom = Rooms.Where(r => r.IsLeaf).OrderBy(x => Guid.NewGuid()).First();
        bossRoom.Data.SetAsBossRoom();
    }

    private void PruneTree()
    {
        foreach (var room in Rooms)
        {
            if (!room.Data.Collapsed)
                continue;
            
            if (room.Data.Collapsed && room.Data.Doors >= room.ChildrenNodes.Count)
                continue;

            var diference = room.ChildrenNodes.Count - room.Data.Doors;

            var branchesToRemove = room.ChildrenNodes.OrderBy(x => Guid.NewGuid()).Take(diference).ToList();

            foreach (var branch in branchesToRemove)
            {
                room.ChildrenNodes.Remove(branch);
                Destroy(branch.Data.gameObject);
            }
        }
    }

    private Tree<DungeonRoom> CreateRoomWithChildren(DungeonRoom baseRoom, int level)
    {
        var room = new Tree<DungeonRoom>(baseRoom);

        if (level >= HowDeep)
            return room;

        level++;

        for (var i = -1; i < MaxDoors - 2; i++)
        {
            var childRoom = InstantiateRoom(i, baseRoom.gameObject);
            room.Add(CreateRoomWithChildren(childRoom, level));
        }

        return room;
    }

    private DungeonRoom InstantiateRoom(int position, GameObject parent)
    {
        var roomObject = Instantiate(
            BaseRoom,
            Vector3.zero, 
            Quaternion.identity);

        roomObject.Init(MaxDoors);
        
        if (parent != null)
        {
            roomObject.transform.parent = parent.transform;
            roomObject.transform.localPosition = parent.transform.position + new Vector3(position + 0.5f * position, 0, 1);
        }

        roomObject.gameObject.SetActive(true);

        return roomObject;
    }
}
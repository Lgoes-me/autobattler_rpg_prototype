using System.Collections.Generic;
using UnityEngine.Rendering;

public class DungeonRoom : SceneData
{
    public bool Collapsed { get; private set; }
    public bool Connected { get; set; }

    public DungeonRoom() : this("", null, new List<Transition>(), MusicType.Dungeon, new List<CombatEncounterData>(),
        null)
    {
        Collapsed = false;
        Connected = false;
    }

    private DungeonRoom(
        string id,
        RoomController roomPrefab,
        List<Transition> doors,
        MusicType music,
        List<CombatEncounterData> combatEncounters,
        VolumeProfile postProcessProfile)
        : base(id, roomPrefab, doors, music, combatEncounters, postProcessProfile)
    {
        Collapsed = false;
        Connected = false;
    }

    public DungeonRoom(
        string id,
        List<Transition> doors,
        RoomController roomPrefab) : this(id, roomPrefab, doors, MusicType.Dungeon, new List<CombatEncounterData>(), null)
    {
        Collapsed = false;
        Connected = false;
    }

    public void SetValue(DungeonRoom dungeonRoom)
    {
        //Id = Guid.NewGuid().ToString();
        //RoomPrefab = dungeonRoom.RoomPrefab;
        Collapsed = true;
    }
}
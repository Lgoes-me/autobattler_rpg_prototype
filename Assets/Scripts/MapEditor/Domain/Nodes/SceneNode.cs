using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SceneNode : BaseSceneNode
{
    public override BaseRoomController Prefab => RoomPrefab;
    public RoomController RoomPrefab { get; }
    public List<CombatEncounterData> CombatEncounters { get; }
    public VolumeProfile PostProcessProfile { get; }
    public List<Transition> Doors { get; }
    public MusicType Music { get; }
    
    private SceneNode(string id) : base(id)
    {
        RoomPrefab = null;
        CombatEncounters = new List<CombatEncounterData>();
        PostProcessProfile = null;
        Doors = new List<Transition>();
        Music = MusicType.Dungeon;
    }
    
    public SceneNode(
        string id,
        List<Transition> doors,
        RoomController roomPrefab,
        List<CombatEncounterData> combatEncounters,
        VolumeProfile postProcessProfile) : this(id)
    {
        Doors = doors;
        RoomPrefab = roomPrefab;
        CombatEncounters = combatEncounters;
        PostProcessProfile = postProcessProfile;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<BaseSceneNode, Spawn> callback)
    {
        callback(this, spawn);
    }

    public override bool IsOpen(Map map, Spawn spawn)
    {
        return true;
    }
}
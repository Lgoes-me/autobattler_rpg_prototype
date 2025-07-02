using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SceneNode : BaseSceneNode
{
    public override BaseRoomController Prefab => RoomPrefab;
    public List<CombatEncounterData> CombatEncounters { get; }
    public List<Transition> Doors { get; }
    
    private RoomController RoomPrefab { get; }
    
    private SceneNode(VolumeProfile postProcessProfile, MusicType music, string id) : base(postProcessProfile, music, id)
    {
        RoomPrefab = null;
        CombatEncounters = new List<CombatEncounterData>();
        Doors = new List<Transition>();
    }
    
    public SceneNode(
        string id,
        List<Transition> doors,
        RoomController roomPrefab,
        List<CombatEncounterData> combatEncounters,
        VolumeProfile postProcessProfile,
        MusicType music) : this(postProcessProfile, music, id)
    {
        Doors = doors;
        RoomPrefab = roomPrefab;
        CombatEncounters = combatEncounters;
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
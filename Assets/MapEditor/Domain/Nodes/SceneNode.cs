using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SceneNode : BaseSceneNode
{
    public RoomController RoomPrefab { get; protected set; }
    public List<CombatEncounterData> CombatEncounters { get; }
    public VolumeProfile PostProcessProfile { get; }
    
    protected SceneNode()
    {
        RoomPrefab = null;
        CombatEncounters = new List<CombatEncounterData>();
        PostProcessProfile = null;
    }
    
    public SceneNode(
        string id,
        List<Transition> doors,
        RoomController roomPrefab,
        List<CombatEncounterData> combatEncounters,
        VolumeProfile postProcessProfile) : base(id, doors)
    {
        RoomPrefab = roomPrefab;
        CombatEncounters = combatEncounters;
        PostProcessProfile = postProcessProfile;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<SceneNode, Spawn> callback)
    {
        callback(this, spawn);
    }

    public override bool IsOpen(Map map, Spawn spawn)
    {
        return true;
    }
}
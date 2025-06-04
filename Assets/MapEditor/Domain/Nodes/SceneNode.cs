using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SceneNode : BaseSceneNode
{
    private RoomController RoomPrefab { get; }
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

    public override void DoTransition(Map map, Spawn spawn, Action<SceneData, Spawn> callback)
    {
        callback(ToSceneData(), spawn);
    }

    public override bool IsOpen(Map map, Spawn spawn)
    {
        return true;
    }

    private SceneData ToSceneData()
    {
        return new SceneData(
            Id,
            RoomPrefab,
            Doors,
            Music,
            CombatEncounters,
            PostProcessProfile);
    }
}
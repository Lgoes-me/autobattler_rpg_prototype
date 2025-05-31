using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Rendering;

public class SceneNode : BaseSceneNode
{
    public RoomController RoomPrefab { get; protected set; }
    public List<CombatEncounterData> CombatEncounters { get; }
    public VolumeProfile PostProcessProfile { get; }
    
    protected SceneNode() : base()
    {
        RoomPrefab = null;
        CombatEncounters = new List<CombatEncounterData>();
        PostProcessProfile = null;
    }
    
    public SceneNode(
        string name, 
        string id,
        List<SpawnDomain> doors,
        RoomController roomPrefab,
        List<CombatEncounterData> combatEncounters,
        VolumeProfile postProcessProfile) : base(name, id, doors)
    {
        RoomPrefab = roomPrefab;
        CombatEncounters = combatEncounters;
        PostProcessProfile = postProcessProfile;
    }

    public override void DoTransition(SpawnDomain spawn, Map map)
    {
        map.SceneManager.EnterRoom(this, spawn);
    }
}
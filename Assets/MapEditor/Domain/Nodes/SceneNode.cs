using System.Collections.Generic;

public class SceneNode : BaseSceneNode
{
    public RoomController RoomPrefab { get; protected set; }
    public List<CombatEncounterData> CombatEncounters { get; protected set; }
    
    protected SceneNode() : base()
    {
        RoomPrefab = null;
        CombatEncounters = new List<CombatEncounterData>();
    }
    
    public SceneNode(
        string name, 
        string id,
        List<DoorData> doors,
        RoomController roomPrefab,
        List<CombatEncounterData> combatEncounters) : base(name, id, doors)
    {
        RoomPrefab = roomPrefab;
        CombatEncounters = combatEncounters;
    }
}
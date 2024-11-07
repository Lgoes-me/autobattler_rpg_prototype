using System.Collections.Generic;

[System.Serializable]
public class Save : ISavable
{
    public string Id { get; set; }
    public SpawnDomain Spawn { get; set; }
    public SpawnDomain LastBonfireSpawn { get; set; }
    public Dictionary<string, PawnInfo> SelectedParty { get; set; }
    public List<string> DefeatedEnemies { get; set; }
    public List<BlessingIdentifier> Blessings { get; set; }
    public List<string> AvailableParty { get; set; }

    public Save()
    {
        Id = "Save.json";
        Spawn = null;
        LastBonfireSpawn = null;
        SelectedParty = new Dictionary<string, PawnInfo>();
        DefeatedEnemies = new List<string>();
        Blessings = new List<BlessingIdentifier>();
        AvailableParty = new List<string>();
    }

    public Save(PawnData startingPlayer)
    {
        Id = "Save.json";
        Spawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        LastBonfireSpawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        SelectedParty = new Dictionary<string, PawnInfo> {{startingPlayer.name, new PawnInfo(startingPlayer.name, 0)}};
        DefeatedEnemies = new List<string>();
        Blessings = new List<BlessingIdentifier>() {BlessingIdentifier.OnAttackHeal};
        AvailableParty = new List<string> {"Guts", "Hunter"};
    }
}
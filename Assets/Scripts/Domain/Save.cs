using System.Collections.Generic;

[System.Serializable]
public class Save : ISavable
{
    public string Id { get; set; }
    public SpawnDomain Spawn { get; set; }
    public SpawnDomain LastBonfireSpawn { get; set; }
    public PawnInfo PlayerPawn { get; set; }
    public Dictionary<string, PawnInfo> SelectedParty { get; set; }
    public List<string> DefeatedEnemies { get; set; }

    public Save()
    {
        Id = "Save.json";
        Spawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        LastBonfireSpawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        PlayerPawn = null;
        SelectedParty = new Dictionary<string, PawnInfo>();
        DefeatedEnemies = new List<string>();
    }

    public Save(PawnData startingPlayer)
    {
        Id = "Save.json";
        Spawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        LastBonfireSpawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        PlayerPawn = new PawnInfo(startingPlayer.name, 0); 
        SelectedParty = new Dictionary<string, PawnInfo>();
        DefeatedEnemies = new List<string>();
    }
}
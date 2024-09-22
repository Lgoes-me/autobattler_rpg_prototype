using System.Collections.Generic;

public class Save : ISavable
{
    public string Id { get; set; }
    public SpawnDomain Spawn { get; set; }
    public SpawnDomain LastBonfireSpawn { get; set; }
    public List<string> SelectedParty { get; set; }
    public List<string> DefeatedEnemies { get; set; }

    public Save()
    {
        Id = "Save.json";
        Spawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        LastBonfireSpawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        SelectedParty = new List<string>();
        DefeatedEnemies = new List<string>();
    }

}
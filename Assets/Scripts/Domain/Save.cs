using System.Collections.Generic;

public class Save : ISavable
{
    public string Id { get; set; }
    
    public string Room { get; set; }
    public string Door { get; set; }
    public List<string> SelectedParty { get; set; }
    public List<string> DefeatedEnemies { get; set; }

    public Save()
    {
        Id = "Save.json";
        
        Room = "DungeonEntrance";
        Door = "DungeonEntrance";
        SelectedParty = new List<string>();
        DefeatedEnemies = new List<string>();
    }

}
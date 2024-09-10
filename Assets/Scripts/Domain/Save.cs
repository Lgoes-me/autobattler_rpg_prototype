using System.Collections.Generic;

public class Save
{
    public string Room { get; set; }
    public string Door { get; set; }
    public List<string> SelectedParty { get; set; }

    public Save()
    {
        Room = "DungeonEntrance";
        Door = "DungeonEntrance";
        SelectedParty = new List<string>();
    }
}
using System.Collections.Generic;

public class SaveState : ISavable<Save>, ILoadable<Save>
{
    public string Id { get; set; }
    
    public string Room { get; set; }
    public string Door { get; set; }
    public List<string> SelectedParty { get; set; }

    public SaveState()
    {
        Id = "SaveState.json";
        SelectedParty = new List<string>();
    }

    public Save LoadData(Save container)
    {
        container.Room = Room;
        container.Door = Door;
        container.SelectedParty = SelectedParty;
        
        return container;
    }

    public void SaveData(Save data)
    {
        Room = data.Room;
        Door = data.Door;
        SelectedParty = data.SelectedParty;
    }
}
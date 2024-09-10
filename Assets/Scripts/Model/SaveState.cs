public class SaveState : ISavable<Save>, ILoadable<Save>
{
    public string Id { get; set; }
    
    public string Room { get; set; }
    public string Door { get; set; }

    public SaveState()
    {
        Id = "SaveState.json";
    }

    public Save LoadData(Save container)
    {
        container.Room = Room;
        container.Door = Door;
        
        return container;
    }

    public void SaveData(Save data)
    {
        Room = data.Room;
        Door = data.Door;
    }
}
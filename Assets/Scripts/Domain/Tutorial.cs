public class Tutorial : ISavable
{
    public Metadata Metadata { get; set; }

    public Tutorial()
    {
        Metadata = new Metadata().CreateNewStaticFile("Tutorial", ".json");
    }
}
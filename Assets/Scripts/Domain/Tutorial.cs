public class Tutorial : ISavable
{
    public string Id { get; set; }
    public bool CompletedOnboard { get; set; }

    public Tutorial()
    {
        Id = "Tutorial.json";
        CompletedOnboard = true;
    }
}
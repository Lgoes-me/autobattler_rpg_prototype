[System.Serializable]
public class SpawnDomain
{
    public string Id { get; private set; }
    public string Scene { get; private set; }
    public string Destiantion { get; private set; }
    public bool Active { get; set; }

    public SpawnDomain(string id, string scene, string destination)
    {
        Id = id;
        Scene = scene;
        Destiantion = destination;
        Active = true;
    }
}
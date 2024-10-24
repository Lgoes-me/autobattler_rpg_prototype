[System.Serializable]
public class SpawnDomain
{
    public string Id { get; set; }
    public string SceneName { get; set; }

    public SpawnDomain(string id, string sceneName)
    {
        Id = id;
        SceneName = sceneName;
    }
}
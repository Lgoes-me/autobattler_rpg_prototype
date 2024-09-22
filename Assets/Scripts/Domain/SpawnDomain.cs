[System.Serializable]
public class SpawnDomain
{
    public string SpawnId { get; set; }
    public string SceneName { get; set; }

    public SpawnDomain(string spawnId, string sceneName)
    {
        SpawnId = spawnId;
        SceneName = sceneName;
    }
}
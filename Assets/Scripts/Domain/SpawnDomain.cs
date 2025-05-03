[System.Serializable]
public class SpawnDomain
{
    public string SpawnId { get; set; }
    public string SceneId { get; set; }
    public bool Active => !string.IsNullOrWhiteSpace(SpawnId) && !string.IsNullOrWhiteSpace(SceneId);

    public SpawnDomain(string spawnId, string sceneId)
    {
        SpawnId = spawnId;
        SceneId = sceneId;
    }
}
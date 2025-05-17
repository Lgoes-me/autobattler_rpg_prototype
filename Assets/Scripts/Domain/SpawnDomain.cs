[System.Serializable]
public class SpawnDomain
{
    public string SpawnId { get; private set; }
    public string SceneId { get; private set; }
    public bool Active => !string.IsNullOrWhiteSpace(SpawnId) && !string.IsNullOrWhiteSpace(SceneId) && Open;
    private bool Open { get; }

    public SpawnDomain(string spawnId, string sceneId, bool open = true)
    {
        SpawnId = spawnId;
        SceneId = sceneId;
        Open = open;
    }
}
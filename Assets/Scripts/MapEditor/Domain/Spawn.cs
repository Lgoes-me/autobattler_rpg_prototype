[System.Serializable]
public class Spawn
{
    public string Id { get; private set; }
    public string NodeId { get; private set; }

    public Spawn(string id, string nodeId)
    {
        Id = id;
        NodeId = nodeId;
    }
}
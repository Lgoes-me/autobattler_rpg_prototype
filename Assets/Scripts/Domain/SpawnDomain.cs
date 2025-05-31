[System.Serializable]
public class SpawnDomain
{
    public string Id { get; private set; }
    public string NodeId { get; private set; }

    public string DestinationId { get; private set; }
    public string DestinationNodeId { get; private set; }

    public bool Active { get; set; }

    public SpawnDomain(string id, string nodeId, string destinationId, string destinationNodeId)
    {
        Id = id;
        NodeId = nodeId;
        DestinationId = destinationId;
        DestinationNodeId = destinationNodeId;
        Active = true;
    }
}
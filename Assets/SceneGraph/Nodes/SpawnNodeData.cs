public class SpawnNodeData : BaseNodeData
{
    public new void Init(string id, string nodeName)
    {
        base.Init(id, nodeName);
    }

    protected override void GenerateDoors()
    {
        var door = new SpawnData
        {
            Name = string.Empty,
            Id = Id
        };

        Doors.Add(door);
    }
}

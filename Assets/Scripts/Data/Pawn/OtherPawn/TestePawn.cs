using System.Collections.Generic;

public class TestePawn
{
    public string Id { get; }
    public List<PawnComponent> Components { get; }

    public TestePawn(string id, List<PawnComponent> components)
    {
        Id = id;
        Components = components;
    }
}
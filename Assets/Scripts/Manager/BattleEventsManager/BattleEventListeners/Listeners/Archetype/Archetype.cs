public class Archetype : BaseIEnumerableGameEventListener
{
    public ArchetypeIdentifier Identifier { get; set; }

    public Archetype(ArchetypeIdentifier identifier)
    {
        Identifier = identifier;
    }
}

public enum ArchetypeIdentifier
{
    Unknown,
    Teste,
}
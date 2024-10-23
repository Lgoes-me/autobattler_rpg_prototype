public class Archetype : BaseIEnumerableGameEventListener
{
    public ArchetypeIdentifier Identifier { get; set; }
    public int Quantidade { get; set; }
    
    public Archetype(ArchetypeIdentifier identifier, int quantidade)
    {
        Identifier = identifier;
        Quantidade = quantidade;
    }
}

public enum ArchetypeIdentifier
{
    Unknown,
    Teste,
}
public class Archetype : BaseIEnumerableGameEventListener
{
    public ArchetypeIdentifier Identifier { get; set; }
    public int CurrentAmount { get; set; }
    public int[] AmountSteps { get; set; }
    
    public Archetype(ArchetypeIdentifier identifier, int currentAmount, int[] amountSteps)
    {
        Identifier = identifier;
        CurrentAmount = currentAmount;
        AmountSteps = amountSteps;
    }
}

public enum ArchetypeIdentifier
{
    Unknown,
    Teste,
}
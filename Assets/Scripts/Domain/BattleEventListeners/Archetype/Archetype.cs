public class Archetype : BaseIEnumerableGameEventListener
{
    public ArchetypeIdentifier Identifier { get; }
    public int CurrentAmount { get; }
    public int[] AmountSteps { get; }
    
    public Archetype(ArchetypeIdentifier identifier, int currentAmount, int[] amountSteps)
    {
        Identifier = identifier;
        CurrentAmount = currentAmount;
        AmountSteps = amountSteps;

        Rarity = Rarity.Deactivated;

        for (var index = 0; index < amountSteps.Length; index++)
        {
            var step = amountSteps[index];
            
            if (CurrentAmount >= step)
            {
                Rarity = (Rarity) index;
            }
        }
    }
}


public enum ArchetypeIdentifier
{
    Unknown,
    Cavaleiros,
    Magos,
    Herois,
    Weakener,
    Hunters,

}
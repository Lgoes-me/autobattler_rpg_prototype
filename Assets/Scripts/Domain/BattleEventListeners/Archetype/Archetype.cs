public class Archetype : BaseIEnumerableGameEventListener
{
    public ArchetypeIdentifier Identifier { get; set; }
    public int CurrentAmount { get; set; }
    public int[] AmountSteps { get; set; }
    
    public Archetype(ArchetypeIdentifier identifier, int currentAmount, int[] amountSteps) : base()
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
                break;
            }
        }
    }
}

public enum Rarity
{
    Deactivated = -1,
    Diamond = 0,
    Gold = 1,
    Silver = 2,
    Bronze = 3,
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
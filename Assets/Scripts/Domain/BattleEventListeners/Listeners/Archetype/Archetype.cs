public class Archetype : BaseIEnumerableGameEventListener
{
    public ArchetypeIdentifier Identifier { get; set; }
    public int CurrentAmount { get; set; }
    public int[] AmountSteps { get; set; }
    public Level CurrentLevel { get; set; }
    
    public Archetype(ArchetypeIdentifier identifier, int currentAmount, int[] amountSteps)
    {
        Identifier = identifier;
        CurrentAmount = currentAmount;
        AmountSteps = amountSteps;

        CurrentLevel = Level.Deactivated;

        for (var index = 0; index < amountSteps.Length; index++)
        {
            var step = amountSteps[index];
            
            if (CurrentAmount >= step)
            {
                CurrentLevel = (Level) index;
                break;
            }
        }
    }
}

public enum Level
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
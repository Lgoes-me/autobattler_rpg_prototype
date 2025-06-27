public class ManaResourceComponent : AbilityResourceComponent
{
    private int ManaCost { get; set; }

    public ManaResourceComponent(PawnController abilityUser, int manaCost) : base(abilityUser)
    {
        ManaCost = manaCost;
    }

    public override void SpendResource()
    {
        var resources = AbilityUser.Pawn.GetComponent<ResourceComponent>();

        if (!resources.HasMana)
            return;

        resources.SpentMana(ManaCost);
    }
}
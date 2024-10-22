public class ManaResourceComponent : AbilityResourceComponent
{
    private int ManaCost { get; set; }

    public ManaResourceComponent(PawnController abilityUser, int manaCost) : base(abilityUser)
    {
        ManaCost = manaCost;
    }

    public override bool HasResource()
    {
        var pawn = AbilityUser.Pawn;
        return pawn.Mana >= ManaCost;
    }

    public override void SpendResource()
    {
        var pawn = AbilityUser.Pawn;

        if (!pawn.HasMana)
            return;

        pawn.SpentMana(ManaCost);
    }
}
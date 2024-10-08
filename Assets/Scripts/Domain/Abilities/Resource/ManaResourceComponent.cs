using UnityEngine;

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
        return pawn.Stats.Mana >= ManaCost;
    }

    public override void SpendResource()
    {
        var pawn = AbilityUser.Pawn;

        if (!pawn.HasMana)
            return;

        pawn.Stats.Mana = Mathf.Clamp(
            ManaCost > 0 ? pawn.Stats.Mana - ManaCost : pawn.Stats.Mana + 10,
            0,
            pawn.Stats.MaxMana);

        AbilityUser.PawnCanvasController.UpdateMana();
    }
}
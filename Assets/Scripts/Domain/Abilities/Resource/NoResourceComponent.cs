using UnityEngine;

public class NoResourceComponent : AbilityResourceComponent
{
    public NoResourceComponent(PawnController abilityUser) : base(abilityUser)
    {
    }

    public override bool HasResource() => true;

    public override void SpendResource()
    {
        var pawn = AbilityUser.Pawn;

        if (!pawn.HasMana)
            return;

        pawn.Stats.Mana = Mathf.Clamp(
            pawn.Stats.Mana + 10,
            0,
            pawn.Stats.MaxMana);

        AbilityUser.PawnCanvasController.UpdateMana();
    }
}
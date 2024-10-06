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

        pawn.Mana = Mathf.Clamp(
            pawn.Mana + 10,
            0,
            pawn.MaxMana);

        AbilityUser.PawnCanvasController.UpdateMana();
    }
}
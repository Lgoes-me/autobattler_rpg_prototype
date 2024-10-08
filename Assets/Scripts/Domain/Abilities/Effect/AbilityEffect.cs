using System.Collections.Generic;

public abstract class AbilityEffect
{
    protected PawnController AbilityUser { get; set; }

    protected AbilityEffect(PawnController abilityUser)
    {
        AbilityUser = abilityUser;
    }

    public void DoAbilityEffect(IEnumerable<PawnController> pawns)
    {
        foreach (var pawn in pawns)
        {
            DoAbilityEffect(pawn);
        }
    }
    
    public abstract void DoAbilityEffect(PawnController pawnController);
}
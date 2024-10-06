using System.Collections.Generic;

public abstract class AbilityEffect
{
    public void DoAbilityEffect(List<PawnController> pawns)
    {
        foreach (var pawn in pawns)
        {
            DoAbilityEffect(pawn);
        }
    }
    
    public abstract void DoAbilityEffect(PawnController pawnController);
}
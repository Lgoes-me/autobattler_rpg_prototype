using System.Collections.Generic;

public abstract class AbilityEffect
{
    public void DoAbilityEffect(IEnumerable<PawnController> pawns)
    {
        foreach (var pawn in pawns)
        {
            DoAbilityEffect(pawn);
        }
    }
    
    public abstract void DoAbilityEffect(PawnController pawnController);
}
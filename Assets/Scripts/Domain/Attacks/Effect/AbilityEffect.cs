public abstract class AbilityEffect
{
    protected PawnController AbilityUser { get; set; }

    protected AbilityEffect(PawnController abilityUser)
    {
        AbilityUser = abilityUser;
    }

    public abstract void DoAbilityEffect(PawnController pawnController);
    
    /*public void AbilityEffect(List<PawnController> pawns)
    {
        foreach (var pawn in pawns)
        {
            //pawn.ReceiveAbilityEffect(Ability);
        }
    }*/
}
public class HealEffect : AbilityEffect
{
    private float HealValue { get; set; }
    private bool CanRevive { get; set; }

    public HealEffect(PawnController abilityUser, float healValue, bool canRevive) : base(abilityUser)
    {
        HealValue = healValue;
        CanRevive = canRevive;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var stats = focus.Pawn.GetComponent<StatsComponent>();
        
        if (!CanRevive && !stats.IsAlive)
            return;
        
        var heal = (int) (HealValue * AbilityUser.Pawn.GetComponent<StatsComponent>().GetPawnStats().Arcane);
        stats.ReceiveHeal(heal, CanRevive);
    }
    
}
public class StaticHealEffect : AbilityEffect
{
    private int HealValue { get; set; }
    private bool CanRevive { get; set; }

    public StaticHealEffect(PawnController abilityUser, int healValue, bool canRevive) : base(abilityUser)
    {
        HealValue = healValue;
        CanRevive = canRevive;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var stats = focus.Pawn.GetComponent<StatsComponent>();

        if (!CanRevive && !stats.IsAlive)
            return;
        
        stats.ReceiveHeal(HealValue, CanRevive);
    }
}
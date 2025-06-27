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
        var resources = AbilityUser.Pawn.GetComponent<ResourceComponent>();

        if (!CanRevive && !resources.IsAlive)
            return;
        
        var heal = (int) (HealValue * AbilityUser.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Arcane));
        resources.ReceiveHeal(heal, CanRevive);
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
        var resources = AbilityUser.Pawn.GetComponent<ResourceComponent>();

        if (!CanRevive && !resources.IsAlive)
            return;
        
        resources.ReceiveHeal(HealValue, CanRevive);
    }
}
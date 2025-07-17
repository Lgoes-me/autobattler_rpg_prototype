using UnityEngine;

public class DamageEffect : AbilityEffect
{
    private DamageDomain Damage { get; set; }

    public DamageEffect(PawnController abilityUser, DamageDomain damage) : base(abilityUser)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var resources = focus.Pawn.GetComponent<ResourceComponent>();

        if (!resources.IsAlive)
            return;

        var vamp = AbilityUser.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Vampirismo);
        
        if (vamp > 0)
        {
            var heal = Mathf.CeilToInt(Damage.Value * vamp / (float) 100);
            AbilityUser.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(heal, false);
        }

        resources.ReceiveDamage(Damage);
    }
}
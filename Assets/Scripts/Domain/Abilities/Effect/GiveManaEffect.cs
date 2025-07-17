using UnityEngine;

public class GiveManaEffect : AbilityEffect
{
    private int ManaValue { get; set; }

    public GiveManaEffect(PawnController abilityUser, int manaValue) : base(abilityUser)
    {
        ManaValue = manaValue;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var resources = focus.Pawn.GetComponent<ResourceComponent>();

        if (!resources.IsAlive)
            return;
        
        resources.GiveMana(ManaValue);
    }
}

public class GivePercentManaEffect : AbilityEffect
{
    private float ManaPercent { get; set; }

    public GivePercentManaEffect(PawnController abilityUser, float manaPercent) : base(abilityUser)
    {
        ManaPercent = manaPercent;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var resources = focus.Pawn.GetComponent<ResourceComponent>();

        if (!resources.IsAlive)
            return;
        
        var maxMana = focus.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Mana);
        var healValue = Mathf.CeilToInt(maxMana * ManaPercent);
        resources.GiveMana(healValue);
    }
}

public class RemoveManaEffect : AbilityEffect
{
    private int ManaValue { get; set; }

    public RemoveManaEffect(PawnController abilityUser, int manaValue) : base(abilityUser)
    {
        ManaValue = manaValue;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var resources = focus.Pawn.GetComponent<ResourceComponent>();

        if (!resources.IsAlive)
            return;
        
        resources.SpentMana(ManaValue);
    }
}

public class RemovePercentManaEffect : AbilityEffect
{
    private float ManaPercent { get; set; }

    public RemovePercentManaEffect(PawnController abilityUser, float manaPercent) : base(abilityUser)
    {
        ManaPercent = manaPercent;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var resources = focus.Pawn.GetComponent<ResourceComponent>();

        if (!resources.IsAlive)
            return;
        
        var maxMana = focus.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Mana);
        var healValue = Mathf.CeilToInt(maxMana * ManaPercent);
        resources.SpentMana(healValue);
    }
}
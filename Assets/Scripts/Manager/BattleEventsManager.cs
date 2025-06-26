public class BattleEventsManager : IManager
{
    private BlessingManager BlessingManager { get; set; }
    private ArchetypeManager ArchetypeManager { get; set; }

    public void Prepare()
    {
        BlessingManager = Application.Instance.GetManager<BlessingManager>();
        ArchetypeManager = Application.Instance.GetManager<ArchetypeManager>();
    }

    public void DoBattleStartEvent(Battle battle)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoBattleStartEvent(battle);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoBattleStartEvent(battle);
        }
    }

    public void DoAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoAttackEvent(battle, abilityUser, ability);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoAttackEvent(battle, abilityUser, ability);
        }
    }

    public void DoSpecialAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoSpecialAttackEvent(battle, abilityUser, ability);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoSpecialAttackEvent(battle, abilityUser, ability);
        }
    }
    
    public void DoPawnDeathEvent(Battle battle, PawnController dead, Pawn attacker)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoPawnDeathEvent(battle, dead, attacker);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoPawnDeathEvent(battle, dead, attacker);
        }
    }
}
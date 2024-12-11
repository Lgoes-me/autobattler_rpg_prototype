public class BattleEventsManager
{
    private BlessingManager BlessingManager { get; set; }
    private PartyManager PartyManager { get; set; }

    public void Prepare()
    {
        BlessingManager = Application.Instance.BlessingManager;
        PartyManager = Application.Instance.PartyManager;
    }

    public void DoBattleStartEvent(Battle battle)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoBattleStartEvent(battle);
        }
        
        foreach (var archetype in PartyManager.Archetypes)
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
        
        foreach (var archetype in PartyManager.Archetypes)
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
        
        foreach (var archetype in PartyManager.Archetypes)
        {
            archetype.DoSpecialAttackEvent(battle, abilityUser, ability);
        }
    }
    
    public void DoPawnDeathEvent(Battle battle, PawnController pawnController)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoPawnDeathEvent(battle, pawnController);
        }
        
        foreach (var archetype in PartyManager.Archetypes)
        {
            archetype.DoPawnDeathEvent(battle, pawnController);
        }
    }
}
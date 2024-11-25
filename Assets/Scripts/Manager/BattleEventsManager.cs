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

    public void DoAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoAttackEvent(abilityUser, ability);
        }
        
        foreach (var archetype in PartyManager.Archetypes)
        {
            archetype.DoAttackEvent(abilityUser, ability);
        }
    }

    public void DoSpecialAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoSpecialAttackEvent(abilityUser, ability);
        }
        
        foreach (var archetype in PartyManager.Archetypes)
        {
            archetype.DoSpecialAttackEvent(abilityUser, ability);
        }
    }
}
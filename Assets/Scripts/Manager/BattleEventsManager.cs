using UnityEngine;

public class BattleEventsManager : MonoBehaviour
{
    [field: SerializeField] private BlessingManager BlessingManager { get; set; }
    [field: SerializeField] private PartyManager PartyManager { get; set; }

    public void DoBattleStartEvent(Battle battle)
    {
        foreach (var archetype in PartyManager.Archetypes)
        {
            archetype.DoBattleStartEvent(battle);
        }

        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoBattleStartEvent(battle);
        }
    }

    public void DoAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (var archetype in PartyManager.Archetypes)
        {
            archetype.DoAttackEvent(abilityUser, ability);
        }

        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoAttackEvent(abilityUser, ability);
        }
    }

    public void DoSpecialAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (var archetype in PartyManager.Archetypes)
        {
            archetype.DoSpecialAttackEvent(abilityUser, ability);
        }

        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoSpecialAttackEvent(abilityUser, ability);
        }
    }
}
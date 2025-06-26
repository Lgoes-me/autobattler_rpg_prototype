using System.Collections;
using System.Collections.Generic;

public abstract class BaseIEnumerableGameEventListener : IEnumerable
{
    private List<BaseBattleEventListener> GameEventListeners { get; set; }
    public Rarity Rarity { get; protected set; }

    protected BaseIEnumerableGameEventListener()
    {
        GameEventListeners = new List<BaseBattleEventListener>();
    }

    public void DoBattleStartEvent(Battle battle)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnBattleStartedListener onBattleStartedListener)
                continue;

            onBattleStartedListener.OnBattleStarted(battle, Rarity);
        }
    }

    public void DoAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnAttackEventListener onAttackEventListener)
                continue;

            onAttackEventListener.OnAttack(battle, abilityUser, ability, Rarity);
        }
    }

    public void DoSpecialAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnSpecialAttackEventListener onSpecialAttackEventListener)
                continue;

            onSpecialAttackEventListener.OnSpecialAttack(battle, abilityUser, ability, Rarity);
        }
    }

    public void DoPawnDeathEvent(Battle battle, PawnController dead, Pawn attacker)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnPawnDeathListener onPawnDeathListener)
                continue;

            onPawnDeathListener.OnPawnDeath(battle, dead, attacker, Rarity);
        }
    }
    
    public void Add(BaseBattleEventListener item)
    {
        GameEventListeners.Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GameEventListeners.GetEnumerator();
    }
}
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

    public void DoBattleFinishedEvent(Battle battle)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnBattleFinishedListener onBattleFinishedListener)
                continue;

            onBattleFinishedListener.OnBattleFinished(battle, Rarity);
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

    public void DoPawnDeathEvent(Battle battle, PawnController dead, DamageDomain damage)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnPawnDeathListener onPawnDeathListener)
                continue;

            onPawnDeathListener.OnPawnDeath(battle, dead, damage, Rarity);
        }
    }
    
    public void DoHealthGainedEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnHealthGainedListener onHealthGainedListener)
                continue;

            onHealthGainedListener.OnHealthGained(battle, pawnController, value, Rarity);
        }
    }
    
    public void DoHealthLostEvent(Battle battle, PawnController pawnController, DamageDomain damage)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnHealthLostListener onHealthLostListener)
                continue;

            onHealthLostListener.OnHealthLost(battle, pawnController, damage, Rarity);
        }
    }
    
    public void DoManaGainedEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnManaGainedListener onManaGainedListener)
                continue;

            onManaGainedListener.OnManaGained(battle, pawnController, value, Rarity);
        }
    }
    
    public void DoManaLostEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not OnManaLostListener onManaLostListener)
                continue;

            onManaLostListener.OnManaLost(battle, pawnController, value, Rarity);
        }
    }
    
    public void DoBlessingGainedEvent()
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not BlessingGainedListener blessingGainedListener)
                continue;

            blessingGainedListener.OnBlessingGained(Rarity);
        }
    }
    
    
    public void DoBlessingCreatedEvent()
    {
        foreach (var listener in GameEventListeners)
        {
            if (listener is not BlessingCreatedListener blessingCreatedListener)
                continue;

            blessingCreatedListener.OnBlessingCreated(Rarity);
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

public enum Rarity
{
    Deactivated = -1,
    Common = 0, // grey
    Uncommon = 1, // green
    Rare = 2, // blue
    Epic = 3, // purple 
    Legendary = 4, // orange 
}
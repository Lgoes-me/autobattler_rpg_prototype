﻿using System.Collections;
using System.Collections.Generic;

public abstract class BaseIEnumerableGameEventListener : IEnumerable
{
    private List<BaseBattleEventListener> GameEventListeners { get; set; }

    protected BaseIEnumerableGameEventListener()
    {
        GameEventListeners = new List<BaseBattleEventListener>();
    }

    public void DoBattleStartEvent(Battle battle)
    {
        foreach (BaseBattleEventListener listener in GameEventListeners)
        {
            if (listener is not OnBattleStartedListener onBattleStartedListener)
                continue;

            onBattleStartedListener.OnBattleStarted(battle);
        }
    }

    public void DoAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (BaseBattleEventListener listener in GameEventListeners)
        {
            if (listener is not OnAttackEventListener onAttackEventListener)
                continue;

            onAttackEventListener.OnAttack(abilityUser, ability);
        }
    }

    public void DoSpecialAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (BaseBattleEventListener listener in GameEventListeners)
        {
            if (listener is not OnSpecialAttackEventListener onSpecialAttackEventListener)
                continue;

            onSpecialAttackEventListener.OnSpecialAttack(abilityUser, ability);
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
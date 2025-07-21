﻿using System;
using UnityEngine;

[CreateAssetMenu]
public class BlessingData : ScriptableObject
{
    [field: SerializeField] public Rarity Rarity { get; private set; }

    [field: SerializeField]
    [field: SerializeReference]
    private BaseBattleEventListenerData[] Events { get; set; }

    public void DoBattleStartEvent(Battle battle)
    {
        foreach (var listener in Events)
        {
            if (listener is not BattleStartedEventListenerData onBattleStartedEventListener)
                continue;

            onBattleStartedEventListener.OnBattleStarted(battle);
        }
    }

    public void DoBattleFinishedEvent(Battle battle)
    {
        foreach (var listener in Events)
        {
            if (listener is not BattleFinishedEventListenerData onBattleFinishedListener)
                continue;

            onBattleFinishedListener.OnBattleFinished(battle);
        }
    }

    public void DoAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var listener in Events)
        {
            if (listener is not AttackEventListenerData onAttackEventListener)
                continue;

            onAttackEventListener.OnAttack(battle, abilityUser, ability);
        }
    }

    public void DoSpecialAttackEvent(Battle battle, PawnController abilityUser, Ability ability)
    {
        foreach (var listener in Events)
        {
            if (listener is not SpecialAttackEventListenerData onSpecialAttackEventListener)
                continue;

            onSpecialAttackEventListener.OnSpecialAttack(battle, abilityUser, ability);
        }
    }

    public void DoPawnDeathEvent(Battle battle, PawnController dead, DamageDomain damage)
    {
        foreach (var listener in Events)
        {
            if (listener is not PawnDeathEventListenerData onPawnDeathListener)
                continue;

            onPawnDeathListener.OnPawnDeath(battle, dead, damage);
        }
    }

    public void DoHealthGainedEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in Events)
        {
            if (listener is not HealthGainedEventListenerData onHealthGainedListener)
                continue;

            onHealthGainedListener.OnHealthGained(battle, pawnController, value);
        }
    }

    public void DoHealthLostEvent(Battle battle, PawnController pawnController, DamageDomain damage)
    {
        foreach (var listener in Events)
        {
            if (listener is not HealthLostEventListenerData onHealthLostListener)
                continue;

            onHealthLostListener.OnHealthLost(battle, pawnController, damage);
        }
    }

    public void DoManaGainedEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in Events)
        {
            if (listener is not ManaGainedEventListenerData onManaGainedListener)
                continue;

            onManaGainedListener.OnManaGained(battle, pawnController, value);
        }
    }

    public void DoManaLostEvent(Battle battle, PawnController pawnController, int value)
    {
        foreach (var listener in Events)
        {
            if (listener is not ManaLostEventListenerData onManaLostListener)
                continue;

            onManaLostListener.OnManaLost(battle, pawnController, value);
        }
    }

    public void DoBlessingCreatedEvent()
    {
        foreach (var listener in Events)
        {
            if (listener is not BlessingCreatedEventListenerData blessingCreatedListener)
                continue;

            blessingCreatedListener.OnBlessingCreated();
        }
    }
}

[Serializable]
public abstract class BaseBattleEventListenerData : IComponentData
{
}

public interface IBlessingEffectData : IComponentData
{
}
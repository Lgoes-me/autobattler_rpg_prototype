using System;
using UnityEngine;

[Serializable]
public class HealthLostEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IHealthLostEffect Effect { get; set; }
    
    public void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage)
        => Effect.OnHealthLost(battle, pawnController, damage);
}

public interface IHealthLostEffect : IBlessingEffectData
{
    void OnHealthLost(Battle battle, PawnController pawnController, DamageDomain damage);
}

[Serializable]
public class HealthGainedEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IHealthGainedEffect Effect { get; set; }
    
    public void OnHealthGained(Battle battle, PawnController pawnController, int value)
        => Effect.OnHealthGained(battle, pawnController, value);
}

public interface IHealthGainedEffect : IBlessingEffectData
{
    void OnHealthGained(Battle battle, PawnController pawnController, int value);
}

[Serializable]
public class ManaGainedEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IManaGainedEffect Effect { get; set; }
    
    public void OnManaGained(Battle battle, PawnController pawnController, int value)
        => Effect.OnManaGained(battle, pawnController, value);
}

public interface IManaGainedEffect : IBlessingEffectData
{
    void OnManaGained(Battle battle, PawnController pawnController, int value);
}


[Serializable]
public class ManaLostEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IManaLostEffect Effect { get; set; }

    public void OnManaLost(Battle battle, PawnController pawnController, int value)
        => Effect.OnManaLost(battle, pawnController, value);
}

public interface IManaLostEffect : IBlessingEffectData
{
    void OnManaLost(Battle battle, PawnController pawnController, int value);
}
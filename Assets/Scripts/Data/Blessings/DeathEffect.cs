using System;
using UnityEngine;

[Serializable]
public class PawnDeathEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IPawnDeathEffect Effect { get; set; }

    public void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage)
        => Effect.OnPawnDeath(battle, pawnController, damage);
}

public interface IPawnDeathEffect : IBlessingEffectData
{
    void OnPawnDeath(Battle battle, PawnController pawnController, DamageDomain damage);
}
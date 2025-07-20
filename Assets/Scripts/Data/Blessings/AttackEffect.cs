using System;
using UnityEngine;

[Serializable]
public class AttackEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IAttackEffect Effect { get; set; }

    public void OnAttack(Battle battle, PawnController abilityUser, Ability ability)
        => Effect.OnAttack(battle, abilityUser, ability);
}

public interface IAttackEffect : IBlessingEffectData
{
    void OnAttack(Battle battle, PawnController abilityUser, Ability ability);
}

[Serializable]
public class SpecialAttackEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private ISpecialAttackEffect Effect { get; set; }
    
    public void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability)
        => Effect.OnSpecialAttack(battle, abilityUser, ability);
}

public interface ISpecialAttackEffect : IBlessingEffectData
{
    void OnSpecialAttack(Battle battle, PawnController abilityUser, Ability ability);
}
using System;
using UnityEngine;

[Serializable]
public class HealToPartyEffectData : IBattleEffect, IDamageReveivedEffect, IAttackEffect, IResourceChangedEffect
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private int HealValue { get; set; }

    public void OnBattleStateChanged(Battle battle) => DoEffect(battle);
    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(battle);
    public void OnAttack(Battle battle, PawnController pawnController, Ability ability) => DoEffect(battle);
    public void OnResourceChanged(Battle battle, PawnController pawnController, int value) => DoEffect(battle);
    
    private void DoEffect(Battle battle)
    {
        var team = Team == TeamType.Player ? battle.PlayerPawns : battle.EnemyPawns;
        
        foreach (var p in team)
        {
            p.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(HealValue, false);
        }
    }
}

[Serializable]
public class HealPawnEffectData : IAttackEffect, IResourceChangedEffect
{
    [field: SerializeField] private int HealValue { get; set; }

    public void OnAttack(Battle battle, PawnController pawnController, Ability ability) => DoEffect(pawnController);
    public void OnResourceChanged(Battle battle, PawnController pawnController, int value) => DoEffect(pawnController);
    
    private void DoEffect(PawnController pawnController)
    {
        pawnController.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(HealValue, false);
    }
}

[Serializable]
public class HealPercentualPlayerPawnEffectData : IDamageReveivedEffect
{
    [field: SerializeField] private int Percentual { get; set; }

    public void OnDamageReceived(Battle battle, PawnController pawnController, DamageDomain damage) => DoEffect(damage);
    
    private void DoEffect(DamageDomain damage)
    {
        if (damage.Attacker == null)
            return;

        var healValue = Mathf.CeilToInt(damage.Value * Percentual / (float) 100);
        damage.Attacker.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false);
    }

}
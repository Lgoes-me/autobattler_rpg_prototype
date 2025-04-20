using System.Linq;
using UnityEngine;

public class DamageAreaEffect : AbilityEffect
{
    private DamageDomain Damage { get; set; }
    private int Range { get; set; }

    public DamageAreaEffect(PawnController abilityUser, DamageDomain damage, int range) : base(abilityUser)
    {
        Damage = damage;
        Range = range;
    }

    public override void DoAbilityEffect(PawnController focus)
    {
        var closePawns = AbilityUser.BattleController.Battle.Pawns
            .Where(p => 
                p.Pawn.Team != AbilityUser.Pawn.Team && 
                Vector3.Distance(AbilityUser.transform.position, p.transform.position) < Range)
            .ToList();

        foreach (var closePawnController in closePawns)
        {
            var pawn = closePawnController.Pawn;

            if (!pawn.IsAlive)
                return;

            pawn.ReceiveDamage(Damage);
        }
    }
}
using System;

public class BlessingFactory
{
    public Blessing CreateBlessing(BlessingIdentifier id)
    {
        return id switch
        {
            BlessingIdentifier.BattleStartGainMana => new Blessing(id)
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) => battle.PlayerPawns.ForEach(p => p.Pawn.GainMana())
                }
            },
            
            BlessingIdentifier.OnAttackHeal => new Blessing(id)
            {
                new OnAttackEventListener()
                {
                    (battle, abilityUser, ability) => IsPlayerTeam(abilityUser),
                    (battle, abilityUser, ability, rarity) =>
                    {
                        abilityUser.Pawn.ReceiveHeal(5, false);
                        abilityUser.ReceiveHeal(false);
                    }
                }
            },
            
            BlessingIdentifier.DamageEnemiesOnEnemyDeath => new Blessing(id)
            {
                new OnPawnDeathListener()
                {
                    (battle, pawnController) => IsEnemyTeam(pawnController),
                    (battle, pawnController, rarity) =>
                    {
                        foreach (var enemyPawn in battle.EnemyPawns)
                        {
                            if(!enemyPawn.Pawn.IsAlive)
                                continue;
                            
                            enemyPawn.Pawn.ReceiveDamage(5);
                            enemyPawn.ReceiveAttack();
                        }
                    }
                }
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }

    private bool IsPlayerTeam(PawnController pawn) => pawn.Pawn.Team == TeamType.Player;
    private bool IsEnemyTeam(PawnController pawn) => pawn.Pawn.Team == TeamType.Enemies;
}
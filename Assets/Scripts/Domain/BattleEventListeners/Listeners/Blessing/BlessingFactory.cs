using System;

public class BlessingFactory
{
    public Blessing CreateBlessing(BlessingIdentifier id, Rarity value)
    {
        return id switch
        {
            BlessingIdentifier.CuraInicioDaLuta => new Blessing(id, value)
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) =>
                    {
                        var healValue = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Diamond => 50,
                            Rarity.Gold => 20,
                            Rarity.Silver => 10,
                            Rarity.Bronze => 5,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };
                        
                        battle.PlayerPawns.ForEach(p => p.Pawn.GetComponent<StatsComponent>().ReceiveHeal(healValue, false));
                    }
                }
            },
            
            
            
            
            
            
            
            BlessingIdentifier.BattleStartGainMana => new Blessing(id, value)
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) => battle.PlayerPawns.ForEach(p => p.Pawn.GetComponent<StatsComponent>().GainMana())
                }
            },
            
            BlessingIdentifier.OnAttackHeal => new Blessing(id, value)
            {
                new OnAttackEventListener()
                {
                    (battle, abilityUser, ability) => IsPlayerTeam(abilityUser),
                    (battle, abilityUser, ability, rarity) =>
                    {
                        abilityUser.Pawn.GetComponent<StatsComponent>().ReceiveHeal(5, false);
                    }
                }
            },
            
            BlessingIdentifier.DamageEnemiesOnEnemyDeath => new Blessing(id, value)
            {
                new OnPawnDeathListener()
                {
                    (battle, pawnController) => IsEnemyTeam(pawnController),
                    (battle, pawnController, rarity) =>
                    {
                        foreach (var enemyPawn in battle.EnemyPawns)
                        {
                            if (!enemyPawn.Pawn.GetComponent<StatsComponent>().IsAlive)
                                continue;
                            
                            enemyPawn.Pawn.GetComponent<StatsComponent>().ReceiveDamage(5);
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
using System;
using UnityEngine;

public class BlessingFactory
{
    // ReSharper disable once CognitiveComplexity
    public Blessing CreateBlessing(BlessingIdentifier id)
    {
        return id switch
        {
            BlessingIdentifier.CuraInicioDaLuta => new Blessing(id)
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

                        battle.PlayerPawns.ForEach(p =>
                            p.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false));
                    }
                }
            },

            BlessingIdentifier.CuraFimDaLuta => new Blessing(id)
            {
                new OnBattleFinishedListener()
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

                        battle.PlayerPawns.ForEach(p =>
                            p.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false));
                    }
                }
            },

            BlessingIdentifier.CuraAoAtacar => new Blessing(id)
            {
                new OnAttackEventListener()
                {
                    (battle, abilityUser, ability) => IsPlayerTeam(abilityUser),
                    (battle, abilityUser, ability, rarity) =>
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

                        abilityUser.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false);
                    }
                }
            },

            BlessingIdentifier.CuraAoGastarMana => new Blessing(id)
            {
                //TODO LISTENER DE GASTAR MANA
                new OnSpecialAttackEventListener()
                {
                    (battle, abilityUser, ability) => IsPlayerTeam(abilityUser),
                    (battle, abilityUser, ability, rarity) =>
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

                        abilityUser.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false);
                    }
                }
            },

            BlessingIdentifier.CuraQuandoAliadoMorre => new Blessing(id)
            {
                new OnPawnDeathListener()
                {
                    (battle, dead, attacker) => IsPlayerTeam(dead),
                    (battle, dead, attacker, rarity) =>
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

                        foreach (var playerPawn in battle.PlayerPawns)
                        {
                            playerPawn.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false);
                        }
                    }
                }
            },

            BlessingIdentifier.CuraQuandoInimigoMorre => new Blessing(id)
            {
                new OnPawnDeathListener()
                {
                    (battle, dead, attacker) => IsPlayerTeam(dead),
                    (battle, dead, attacker, rarity) =>
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

                        foreach (var playerPawn in battle.PlayerPawns)
                        {
                            playerPawn.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false);
                        }
                    }
                }
            },

            BlessingIdentifier.CuraAoLongoDoTempo => new Blessing(id)
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

                        var buff = new Buff(BlessingIdentifier.CuraAoLongoDoTempo.ToString(), -1)
                        {
                            new RegenBuff(healValue, 2)
                        };

                        GiveBuffToPlayerTeam(battle, buff);
                    }
                }
            },

            BlessingIdentifier.AumentaCuraPercentualmente => new Blessing(id)
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) =>
                    {
                        var healPowerValue = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Diamond => 20,
                            Rarity.Gold => 10,
                            Rarity.Silver => 4,
                            Rarity.Bronze => 1,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };

                        var stats = new StatsData()
                        {
                            new StatData(Stat.HealPower, healPowerValue),
                        };

                        var buff = new Buff(BlessingIdentifier.AumentaCuraPercentualmente.ToString(), -1)
                        {
                            new StatModifierBuff(stats.ToDomain())
                        };

                        GiveBuffToPlayerTeam(battle, buff);
                    }
                }
            },

            BlessingIdentifier.BonusDeStatQuandoCuraAcontece => new Blessing(id)
            {
                //TODO LISTENER DE GANHAR VIDA
            },

            BlessingIdentifier.RevivePrimeiroAliadoAMorrerEmCombate => new Blessing(id)
            {
                new OnPawnDeathListener()
                {
                    (battle, dead, attacker) =>
                        IsPlayerTeam(dead) &&
                        !dead.Pawn.GetComponent<MetaDataComponent>()
                            .CheckMetaData(BlessingIdentifier.RevivePrimeiroAliadoAMorrerEmCombate.ToString()),

                    (battle, dead, attacker, rarity) =>
                    {
                        var percentualHealValue = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Diamond => 100,
                            Rarity.Gold => 75,
                            Rarity.Silver => 50,
                            Rarity.Bronze => 15,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };

                        var health = dead.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Health);
                        var healValue = Mathf.CeilToInt(health * percentualHealValue / (float) 100);
                        dead.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, true);

                        GiveMetaDataToPlayerTeam(battle,
                            BlessingIdentifier.RevivePrimeiroAliadoAMorrerEmCombate.ToString());
                    }
                }
            },

            BlessingIdentifier.ReviveTodosAliadosAMorreremEmCombate => new Blessing(id)
            {
                new OnPawnDeathListener()
                {
                    (battle, dead, attacker) =>
                        IsPlayerTeam(dead) &&
                        !dead.Pawn.GetComponent<MetaDataComponent>()
                            .CheckMetaData(BlessingIdentifier.RevivePrimeiroAliadoAMorrerEmCombate.ToString()),

                    (battle, dead, attacker, rarity) =>
                    {
                        var percentualHealValue = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Diamond => 100,
                            Rarity.Gold => 75,
                            Rarity.Silver => 50,
                            Rarity.Bronze => 15,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };

                        var health = dead.Pawn.GetComponent<StatsComponent>().GetStats().GetStat(Stat.Health);
                        var healValue = Mathf.CeilToInt(health * percentualHealValue / (float) 100);
                        
                        dead.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, true);
                        dead.Pawn.GetComponent<MetaDataComponent>().AddMetaData(BlessingIdentifier.RevivePrimeiroAliadoAMorrerEmCombate.ToString());
                    }
                }
            },

            BlessingIdentifier.DanoEmAreaQuandoAliadoMorre => new Blessing(id)
            {
                //TODO Aoe
                new OnPawnDeathListener()
                {
                    (battle, dead, attacker) => IsPlayerTeam(dead),
                    (battle, dead, attacker, rarity) =>
                    {
                        var damage = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Diamond => 100,
                            Rarity.Gold => 75,
                            Rarity.Silver => 50,
                            Rarity.Bronze => 15,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };

                        foreach (var enemyPawn in battle.EnemyPawns)
                        {
                            if (!enemyPawn.Pawn.GetComponent<ResourceComponent>().IsAlive)
                                continue;

                            enemyPawn.Pawn.GetComponent<ResourceComponent>().ReceiveDamage(damage);
                        }
                    }
                }
            },

            BlessingIdentifier.DanoEmAreaQuandoInimigoMorre => new Blessing(id)
            {
                //TODO Aoe
                new OnPawnDeathListener()
                {
                    (battle, dead, attacker) => IsEnemyTeam(dead),
                    (battle, dead, attacker, rarity) =>
                    {
                        var damage = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Diamond => 100,
                            Rarity.Gold => 75,
                            Rarity.Silver => 50,
                            Rarity.Bronze => 15,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };

                        foreach (var enemyPawn in battle.EnemyPawns)
                        {
                            if (!enemyPawn.Pawn.GetComponent<ResourceComponent>().IsAlive)
                                continue;

                            enemyPawn.Pawn.GetComponent<ResourceComponent>().ReceiveDamage(damage);
                        }
                    }
                }
            },

            BlessingIdentifier.DanoDeVingançaNoInimigoQuandoAliadoMorre => new Blessing(id)
            {
                new OnPawnDeathListener()
                {
                    (battle, dead, attacker) => attacker != null && IsPlayerTeam(dead),
                    (battle, dead, attacker, rarity) =>
                    {
                        var damage = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Diamond => 100,
                            Rarity.Gold => 75,
                            Rarity.Silver => 50,
                            Rarity.Bronze => 15,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };
                        
                        attacker.GetComponent<ResourceComponent>().ReceiveDamage(damage);
                    }
                }
            },

            BlessingIdentifier.BattleStartGainMana => new Blessing(id)
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) =>
                        battle.PlayerPawns.ForEach(p => p.Pawn.GetComponent<ResourceComponent>().GainMana())
                }
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }

    private bool IsPlayerTeam(PawnController pawn) => pawn.Pawn.Team == TeamType.Player;
    private bool IsEnemyTeam(PawnController pawn) => pawn.Pawn.Team == TeamType.Enemies;

    private void GiveBuffToPlayerTeam(Battle battle, Buff buff)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }

    private void GiveMetaDataToPlayerTeam(Battle battle, string data)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<MetaDataComponent>().AddMetaData(data);
        }
    }
}
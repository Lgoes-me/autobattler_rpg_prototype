using System;

public class ArchetypeFactory
{
    public Archetype CreateArchetype(ArchetypeIdentifier id, int currentAmount)
    {
        return id switch
        {
            ArchetypeIdentifier.Cavaleiros => new Archetype(id, currentAmount, new[] {2, 4, 6})
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) =>
                    {
                        if (rarity is Rarity.Deactivated)
                            return;
                        
                        var s = new StatsData()
                        {
                            new StatData(Stat.PhysicalDefence, 1)
                        };

                        var b = new Buff("ProtecaoCavalereica", -1)
                        {
                            new StatModifierBuff(s.ToDomain())
                        };
                        
                        GiveBuffToPlayerTeam(battle, b);

                        var s2 = new StatsData()
                        {
                            new StatData(Stat.PhysicalDefence, 1)
                        };

                        var b2 = new Buff("Cavaleiros", -1)
                        {
                            new StatModifierBuff(s2.ToDomain())
                        };

                        GiveBuffToArchetypeInPlayerTeam(battle, b2, ArchetypeIdentifier.Cavaleiros);
                    }
                }
            },

            ArchetypeIdentifier.Magos => new Archetype(id, currentAmount, new[] {1, 4, 6})
            {
                new OnSpecialAttackEventListener()
                {
                    (battle, _, _, rarity) =>
                    {
                        if (rarity is Rarity.Deactivated)
                            return;

                        var s = new StatsData()
                        {
                            new StatData(Stat.Arcane, 1)
                        };
                        
                        var b = new Buff("Magos", -1)
                        {
                            new StatModifierBuff(s.ToDomain())
                        };

                        GiveBuffToArchetypeInPlayerTeam(battle, b, ArchetypeIdentifier.Magos);
                    }
                }
            },

            ArchetypeIdentifier.Herois => new Archetype(id, currentAmount, new[] {1, 8})
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) =>
                    {
                        if (rarity is Rarity.Deactivated)
                            return;

                        var s = new StatsData()
                        {
                            new StatData(Stat.Strength, 1),
                            new StatData(Stat.Arcane, 1),
                            new StatData(Stat.PhysicalDefence, 1),
                            new StatData(Stat.MagicalDefence, 1),
                        };
                        
                        var b = new Buff("Herois", -1)
                        {
                            new StatModifierBuff(s.ToDomain())
                        };
                        
                        GiveBuffToArchetypeInPlayerTeam(battle, b, ArchetypeIdentifier.Herois);
                    }
                }
            },
            

            ArchetypeIdentifier.Hunters => new Archetype(id, currentAmount, new[] {2, 8})
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) =>
                    {
                        if (rarity is Rarity.Deactivated)
                            return;

                        var s = new StatsData()
                        {
                            new StatData(Stat.PhysicalDefence, -2),
                        };
                        
                        var b = new Buff("Hunters", -1)
                        {
                            new StatModifierBuff(s.ToDomain())
                        };
                        
                        GiveBuffToEnemyTeam(battle, b);
                    }
                }
            },
            
            ArchetypeIdentifier.Weakener => new Archetype(id, currentAmount, new[] {1, 8})
            {
                new OnBattleStartedListener()
                {
                    (battle, rarity) =>
                    {
                        if (rarity is Rarity.Deactivated)
                            return;

                        var s = new StatsData()
                        {
                            new StatData(Stat.Strength, -3),
                        };
                        
                        var b = new Buff("Weakener", -1)
                        {
                            new StatModifierBuff(s.ToDomain())
                        };
                        
                        GiveBuffToEnemyTeam(battle,b);
                    }
                }
            },
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }

    private void GiveBuffToPlayerTeam(Battle battle, Buff buff)
    {
        foreach (var p in battle.PlayerPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
    
    private void GiveBuffToEnemyTeam(Battle battle, Buff buff)
    {
        foreach (var p in battle.EnemyPawns)
        {
            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }

    private void GiveBuffToArchetypeInPlayerTeam(Battle battle, Buff buff, ArchetypeIdentifier archetypeIdentifier)
    {
        foreach (var p in battle.PlayerPawns)
        {
            if (!p.Pawn.GetComponent<ArchetypesComponent>().Archetypes.Contains(archetypeIdentifier))
                continue;

            p.Pawn.GetComponent<PawnBuffsComponent>().AddBuff(buff);
        }
    }
}
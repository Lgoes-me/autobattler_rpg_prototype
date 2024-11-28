﻿using System;

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
                    battle =>
                    {
                        var s = new StatsData(0, 0, 2, 0);
                        var b = new StatModifierBuff(s.ToDomain(), "ProtecaoCavalereica", -1);
                        GiveBuffToPlayerTeam(battle, b);

                        var s2 = new StatsData(0, 0, 10, 0);
                        var b2 = new StatModifierBuff(s2.ToDomain(), "Cavaleiros", -1);

                        GiveBuffToArchetypeInPlayerTeam(battle, b2, ArchetypeIdentifier.Cavaleiros);
                    }
                }
            },

            ArchetypeIdentifier.Magos => new Archetype(id, currentAmount, new[] {1, 4, 6})
            {
                new OnSpecialAttackEventListener()
                {
                    (battle, user, ability) =>
                    {
                        var s = new StatsData(0, 1, 0, 0);
                        var b = new StatModifierBuff(s.ToDomain(), "Magos", -1, true);

                        GiveBuffToArchetypeInPlayerTeam(battle, b, ArchetypeIdentifier.Magos);
                    }
                }
            },

            ArchetypeIdentifier.Herois => new Archetype(id, currentAmount, new[] {1, 8})
            {
                new OnBattleStartedListener()
                {
                    battle =>
                    {
                        var s = new StatsData(1, 1, 1, 1);
                        var b = new StatModifierBuff(s.ToDomain(), "Herois", -1);

                        GiveBuffToArchetypeInPlayerTeam(battle, b, ArchetypeIdentifier.Herois);
                    }
                }
            },
            
            ArchetypeIdentifier.Weakener => new Archetype(id, currentAmount, new[] {1, 8})
            {
                new OnBattleStartedListener()
                {
                    battle =>
                    {
                        var s = new StatsData(-3, 0, 0, 0);
                        var b = new StatModifierBuff(s.ToDomain(), "Weakener", -1);

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
            p.Pawn.AddBuff(buff);
        }
    }
    
    private void GiveBuffToEnemyTeam(Battle battle, Buff buff)
    {
        foreach (var p in battle.EnemyPawns)
        {
            p.Pawn.AddBuff(buff);
        }
    }

    private void GiveBuffToArchetypeInPlayerTeam(Battle battle, Buff buff, ArchetypeIdentifier archetypeIdentifier)
    {
        foreach (var p in battle.PlayerPawns)
        {
            if (!p.Pawn.Archetypes.Contains(archetypeIdentifier))
                continue;

            p.Pawn.AddBuff(buff);
        }
    }
}
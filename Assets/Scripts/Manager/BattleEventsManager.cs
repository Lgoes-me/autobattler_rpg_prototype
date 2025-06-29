using System.Collections.Generic;

public class BattleEventsManager : IManager
{
    private BlessingManager BlessingManager { get; set; }
    private ArchetypeManager ArchetypeManager { get; set; }
    private PawnController Boss { get; set; }
    private List<BossModifier> BossModifiers { get; set; }
    private Battle Battle { get; set; }

    public void Prepare()
    {
        BlessingManager = Application.Instance.GetManager<BlessingManager>();
        ArchetypeManager = Application.Instance.GetManager<ArchetypeManager>();
    }

    public void SetBoss(PawnController boss, List<BossModifier> bossModifiers)
    {
        Boss = boss;
        BossModifiers = bossModifiers;
    }

    public void PrepareBattle(Battle battle)
    {
        Battle = battle;
    }

    public void FinishBattle()
    {
        Battle = null;
        Boss = null;
        BossModifiers = null;
    }

    public void DoBattleStartEvent()
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoBattleStartEvent(Battle);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoBattleStartEvent(Battle);
        }

        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoBattleStartEvent(Battle);
            }
        }
    }

    public void DoBattleFinishedEvent()
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoBattleFinishedEvent(Battle);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoBattleFinishedEvent(Battle);
        }

        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoBattleFinishedEvent(Battle);
            }
        }
    }
    
    public void DoAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoAttackEvent(Battle, abilityUser, ability);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoAttackEvent(Battle, abilityUser, ability);
        }
        
        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoAttackEvent(Battle, abilityUser, ability);
            }
        }
    }

    public void DoSpecialAttackEvent(PawnController abilityUser, Ability ability)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoSpecialAttackEvent(Battle, abilityUser, ability);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoSpecialAttackEvent(Battle, abilityUser, ability);
        }
        
        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoSpecialAttackEvent(Battle, abilityUser, ability);
            }
        }
    }
    
    public void DoPawnDeathEvent(PawnController dead, DamageDomain damage)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoPawnDeathEvent(Battle, dead, damage);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoPawnDeathEvent(Battle, dead, damage);
        } 
        
        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoPawnDeathEvent(Battle, dead, damage);
            }
        }
    }
    
    public void DoHealthGainedEvent(PawnController pawnController, int value)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoHealthGainedEvent(Battle, pawnController, value);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoHealthGainedEvent(Battle, pawnController, value);
        }
        
        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoHealthGainedEvent(Battle, pawnController, value);
            }
        }
    }
    
    public void DoHealthLostEvent(PawnController pawnController, DamageDomain damage)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoHealthLostEvent(Battle, pawnController, damage);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoHealthLostEvent(Battle, pawnController, damage);
        }
        
        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoHealthLostEvent(Battle, pawnController, damage);
            }
        }
    }
    
    public void DoManaGainedEvent(PawnController pawnController, int value)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoManaGainedEvent(Battle, pawnController, value);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoManaGainedEvent(Battle, pawnController, value);
        }
        
        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoManaGainedEvent(Battle, pawnController, value);
            }
        }
    }
    
    public void DoManaLostEvent(PawnController pawnController, int value)
    {
        foreach (var blessing in BlessingManager.Blessings)
        {
            blessing.DoManaLostEvent(Battle, pawnController, value);
        }
        
        foreach (var archetype in ArchetypeManager.Archetypes)
        {
            archetype.DoManaLostEvent(Battle, pawnController, value);
        }
        
        if (Boss != null && Boss.Pawn.GetComponent<ResourceComponent>().IsAlive)
        {
            foreach (var bossModifier in BossModifiers)
            {
                bossModifier.DoManaLostEvent(Battle, pawnController, value);
            }
        }
    }
}
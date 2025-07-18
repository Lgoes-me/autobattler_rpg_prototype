using System;
using System.Collections.Generic;
using System.Linq;

public class Pawn
{
    public string Id { get; }
    private Dictionary<Type, PawnComponent> Components { get; set; }

    public delegate void PawnDomainChanged();
    public delegate void PawnDomainChanged<in T>(T param);
    
    public event PawnDomainChanged BattleStarted;
    public event PawnDomainChanged BattleFinished;
    
    private PawnStatus Status { get; set; }
    public TeamType Team { get; }
    public WeaponType WeaponType { get; private set; }
    
    public PawnController Focus { get; set; }

    public Pawn(string id, List<PawnComponent> components)
    {
        foreach (var component in components)
        {
            component.Init(this);
        }

        Id = id;
        Components = components.ToDictionary(c => c.GetType(), c => c);
        
        var metaDataComponent = new MetaDataComponent();
        metaDataComponent.Init(this);
        AddComponent(metaDataComponent);
        
        if (HasComponent<StatsComponent>())
        {
            var resourceComponent = new ResourceComponent();
            resourceComponent.Init(this);
            AddComponent(resourceComponent);
        }
    }

    public Pawn(
        string id,
        List<PawnComponent> components,
        PawnStatus status,  
        TeamType team) : this(id, components)
    {
        Status = status;
        Team = team;
    }
    
    public void SetWeaponType(WeaponType weaponType)
    {
        WeaponType = weaponType;
    }

    public void AddComponent<T>(T component) where T : PawnComponent
    {
        if (Components.TryGetValue(typeof(T), out var _))
        {
            throw new Exception($"Já foi cadastrada o componente {typeof(T)} no Pawn");
        }

        Components.Add(typeof(T), component);
    }

    public T GetComponent<T>() where T : PawnComponent
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return (T) component;
        }

        throw new Exception($"Não foi cadastrada o componente {typeof(T)} no Pawn");
    }

    public bool HasComponent<T>() where T : PawnComponent
    {
        return Components.ContainsKey(typeof(T));
    }

    public bool TryGetComponent<T>(out T result) where T : PawnComponent
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            result = (T) component;
            return true;
        }

        result = null;
        return false;
    }

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Status = pawnInfo.Status;

        foreach (var (_, component) in Components)
        {
            component.SetPawnInfo(pawnInfo);
        }
    }

    public PawnInfo ResetPawnInfo()
    {
        var pawnInfo = new PawnInfo(
            Id,
            1,
            0,
            0,
            Status,
            GetComponent<WeaponComponent>().Weapon,
            GetComponent<AbilitiesComponent>().Abilities,
            GetComponent<ConsumableComponent>().Consumables,
            GetComponent<PawnBuffsComponent>().PermanentBuffs);

        SetPawnInfo(pawnInfo);
        return pawnInfo;
    }

    public PawnInfo GetPawnInfo()
    {
        return new PawnInfo(
            Id,
            TryGetComponent<LevelUpStatsComponent>(out var stats) ? stats.Level : 1,
            GetComponent<ResourceComponent>().MissingHealth,
            GetComponent<ResourceComponent>().Experience,
            Status,
            GetComponent<WeaponComponent>().Weapon,
            GetComponent<AbilitiesComponent>().Abilities,
            GetComponent<ConsumableComponent>().Consumables,
            GetComponent<PawnBuffsComponent>().PermanentBuffs);
    }

    public void StartBattle()
    {
        BattleStarted?.Invoke();
        GetComponent<ResourceComponent>().StartBattle();
        GetComponent<PawnBuffsComponent>().StartBattle();
    }
    
    public void EndBattle()
    {
        BattleFinished?.Invoke();
        GetComponent<MetaDataComponent>().EndBattle();
        GetComponent<PawnBuffsComponent>().EndBattle();
    }
}

public enum TeamType
{
    Player,
    Enemies
}
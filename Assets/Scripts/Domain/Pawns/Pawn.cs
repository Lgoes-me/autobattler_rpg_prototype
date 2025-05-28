using System;
using System.Collections.Generic;
using System.Linq;

public class Pawn
{
    public string Id { get; }
    private Dictionary<Type, PawnComponent> Components { get; set; }

    private PawnStatus Status { get; set; }
    public TeamType Team { get; }
    public int Level { get; }
    public WeaponType WeaponType { get; }
    
    public PawnController Focus { get; set; }

    public Pawn(string id, List<PawnComponent> components)
    {
        foreach (var component in components)
        {
            component.Init(this);
        }

        Id = id;
        Components = components.ToDictionary(c => c.GetType(), c => c);
    }

    public Pawn(
        string id,
        List<PawnComponent> components,
        PawnStatus status,
        TeamType team,
        int level,
        WeaponType weaponType, 
        MountComponent mount) : this(id, components)
    {
        Status = status;
        Team = team;
        Level = level;
        WeaponType = weaponType;

        if (mount != null && !HasComponent<MountComponent>())
        {
            AddComponent(mount);
        }
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
            GetComponent<StatsComponent>().Level,
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
        var stats = GetComponent<StatsComponent>();

        return new PawnInfo(
            Id,
            stats.Level,
            stats.GetPawnStats().Health - stats.Health,
            Status,
            GetComponent<WeaponComponent>().Weapon,
            GetComponent<AbilitiesComponent>().Abilities,
            GetComponent<ConsumableComponent>().Consumables,
            GetComponent<PawnBuffsComponent>().PermanentBuffs);
    }
}

public enum TeamType
{
    Player,
    Enemies
}
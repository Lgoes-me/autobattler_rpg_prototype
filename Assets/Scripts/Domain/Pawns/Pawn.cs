using System;
using System.Collections.Generic;
using System.Linq;

public class Pawn
{
    public string Id { get; }
    private Dictionary<Type, PawnComponent> Components { get; set; }

    public TeamType Team { get; }
    public PawnStatus Status { get; private set; }

    public Pawn(string id, List<PawnComponent> components)
    {
        Id = id;
        Components = components.ToDictionary(c => c.GetType(), c => c);
        Status = PawnStatus.Main;
    }

    public Pawn(string id, List<PawnComponent> components, TeamType team, int level) : this(id, components)
    {
        Team = team;
        //GetComponent<StatsComponent>().ApplyLevel(level);
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

    public void SetPawnInfo(PawnInfo pawnInfo)
    {
        Status = pawnInfo.Status;

        GetComponent<StatsComponent>().SetPawnInfo(pawnInfo);

        if (pawnInfo.Weapon != null)
        {
            GetComponent<WeaponComponent>().SetPawnInfo(pawnInfo);
        }

        GetComponent<AbilitiesComponent>().SetPawnInfo(pawnInfo);
    }

    public PawnInfo ResetPawnInfo()
    {
        var pawnInfo = new PawnInfo(
            Id,
            GetComponent<StatsComponent>().Level,
            0,
            Status,
            GetComponent<WeaponComponent>().Weapon,
            GetComponent<AbilitiesComponent>().Abilities);


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
            GetComponent<AbilitiesComponent>().Abilities);
    }
}

public enum TeamType
{
    Player,
    Enemies
}
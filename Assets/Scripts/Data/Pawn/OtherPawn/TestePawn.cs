using System;
using System.Collections.Generic;
using System.Linq;

public class TestePawn
{
    public string Id { get; }
    private Dictionary<Type, PawnComponent> Components { get; set; }

    public TestePawn(string id, List<PawnComponent> components)
    {
        Id = id;
        Components = components.ToDictionary(c => c.GetType(), c => c);
    }

    public T GetComponent<T>() where T : PawnComponent
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return (T) component;
        }

        throw new Exception($"Não foi cadastrada o componente {typeof(T)} no Pawn");
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuffData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeReference] [field: SerializeField] private List<BuffComponentData> Buffs { get; set; }
    
    [field: SerializeField] private float Duration { get; set; }

    public Buff ToDomain(Pawn pawn)
    {
        var buff = new Buff(Id, Duration);

        foreach (var buffComponentData in Buffs)
        {
            var buffComponent = buffComponentData.ToDomain(pawn);
            buff.Add(buffComponent);
        }
        
        return new Buff(Id, Duration);
    }
}

[Serializable]
public abstract class BuffComponentData : IComponentData
{
    public abstract BuffComponent ToDomain(Pawn pawn);
}
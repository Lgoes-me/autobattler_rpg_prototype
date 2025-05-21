using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuffData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeReference] [field: SerializeField] private List<BuffComponentData> Buffs { get; set; }
    
    public Buff ToDomain(Pawn pawn, float duration)
    {
        var buff = new Buff(Id, duration);

        foreach (var buffComponentData in Buffs)
        {
            var buffComponent = buffComponentData.ToDomain(pawn);
            buff.Add(buffComponent);
        }
        
        return buff;
    }
}

[Serializable]
public abstract class BuffComponentData : IComponentData
{
    public abstract BuffComponent ToDomain(Pawn pawn);
}
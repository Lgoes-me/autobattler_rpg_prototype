using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class EnemyDataComponent : IComponentData
{
    public abstract EnemyDataComponent ToDomain();
}

public class BossComponentData : EnemyDataComponent
{
    [field: SerializeField] private List<BossModifier> Modifiers { get; set; }
    
    public override EnemyDataComponent ToDomain()
    {
        return null; //new BossComponent();
    }
}
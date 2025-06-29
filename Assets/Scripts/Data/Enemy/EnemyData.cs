using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyData
{
    [field: HideInInspector] public string name;
    public PawnController PawnController { get; set; }

    [field: SerializeReference] [field: SerializeField] private List<EnemyDataComponent> Components { get; set; }
    
    public void PreparePawn(PawnController pawnController)
    {
        PawnController = pawnController;

        foreach (var component in Components)
        {
            component.PreparePawn(PawnController);
        }
    }
    
    public bool IsBoss(out BossComponentData bossComponentData)
    {
        bossComponentData = Components.FirstOrDefault(item => item is BossComponentData) as BossComponentData;
        return bossComponentData != null;
    }
}
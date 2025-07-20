using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyData
{
    [field: HideInInspector] public string name;
    public PawnController PawnController { get; private set; }

    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<EnemyDataComponent> Components { get; set; }
    
    public void PreparePawn(PawnController pawnController, TeamType team)
    {
        PawnController = pawnController;
        PawnController.Init(PawnData.ToDomain(PawnStatus.Battle, team));
        
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
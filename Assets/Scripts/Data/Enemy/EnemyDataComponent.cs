using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class EnemyDataComponent : IComponentData
{
    public virtual void PreparePawn(PawnController pawnController)
    {

    }
}

public class BossComponentData : EnemyDataComponent
{
    [field: SerializeField] public List<BossModifierData> Modifiers { get; private set; }
}

public class WeaponComponentData : EnemyDataComponent
{
    [field: SerializeField] private WeaponType WeaponType { get; set; }

    public override void PreparePawn(PawnController pawnController)
    {
        pawnController.Pawn.SetWeaponType(WeaponType);
    }
}

public class MountedEnemyComponentData : EnemyDataComponent
{
    [field: SerializeField] private CharacterController Mount { get; set; }

    public override void PreparePawn(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;
        
        if (!pawn.HasComponent<MountComponent>())
        {
            var mountComponent = new MountComponent(Mount);
            mountComponent.Init(pawn);
            pawn.AddComponent(mountComponent);
        }
    }
}

public class LevelComponentData : EnemyDataComponent
{
    [field: SerializeField] public int Level { get; set; }

    public override void PreparePawn(PawnController pawnController)
    {
        if (pawnController.Pawn.TryGetComponent<LevelUpStatsComponent>(out var statsComponent))
        {
            statsComponent.ApplyLevel(Level);
        }
    }
}

public class AnimationComponentData : EnemyDataComponent
{
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private bool Forward { get; set; }

    public override void PreparePawn(PawnController pawnController)
    {
        pawnController.CharacterController.SetDirection((Forward ? 1 : -1) * pawnController.transform.forward);
        pawnController.CharacterController.SetAnimationState(
            new DefaultState(Animation, () => pawnController.CharacterController.SetAnimationState(new IdleState())));
    }
}
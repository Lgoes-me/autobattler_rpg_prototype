using UnityEngine;

[System.Serializable]
public abstract class PriorityModifier : IComponentData
{
    [field: SerializeField] protected int Multiplier { get; set; } = 1;
    
    public abstract int AlterPriority(
        PawnController abilityUser,
        Battle battle,
        FocusData focusData,
        int priority);
}
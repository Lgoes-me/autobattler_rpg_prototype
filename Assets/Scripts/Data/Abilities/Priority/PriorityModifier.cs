using UnityEngine;

[System.Serializable]
public abstract class PriorityModifier : AbilityComponentData
{
    [field: SerializeField] protected int Multiplier { get; set; } = 1;
    
    public abstract int AlterPriority(
        PawnController abilityUser,
        Battle battle,
        BaseFocusData focusData,
        int priority);
}
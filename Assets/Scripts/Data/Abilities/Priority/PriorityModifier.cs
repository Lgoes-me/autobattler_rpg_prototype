using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PriorityModifier : AbilityComponentData
{
    [field: SerializeField] protected int Multiplier { get; set; } = 1;
    
    public abstract int AlterPriority(
        PawnController abilityUser,
        List<PawnController> pawns,
        BaseFocusData focusData,
        int priority);
}
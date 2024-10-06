using System.Collections.Generic;

[System.Serializable]
public abstract class PriorityModifier : AbilityComponentData
{
    public abstract int AlterPriority(PawnController abilityUser, List<PawnController> pawns, int priority);
}
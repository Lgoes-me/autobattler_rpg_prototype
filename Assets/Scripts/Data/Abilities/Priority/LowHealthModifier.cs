using UnityEngine;

[System.Serializable]
public class LowHealthModifier : PriorityModifier
{
    [field: SerializeField] private int Health { get; set; }
    
    public override int AlterPriority(PawnController abilityUser, Battle battle, int priority)
    {
        if (abilityUser.Pawn.Health < Health)
            return priority + 1 * Multiplier;
        
        return priority;
    }
}
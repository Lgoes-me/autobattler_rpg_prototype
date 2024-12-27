using UnityEngine;

[System.Serializable]
public class LowHealthModifier : PriorityModifier
{
    [field: SerializeField] private int Health { get; set; }
    
    public override int AlterPriority(PawnController abilityUser, Battle battle, FocusData focusData, int priority)
    {
        //var selectedPawn = battle.Query(abilityUser, focusData.Target, focusData.Focus, 0);
        
        //if (selectedPawn != null && selectedPawn.Pawn.Health < Health)
            //return priority + 1 * Multiplier;
        
        return priority;
    }
}
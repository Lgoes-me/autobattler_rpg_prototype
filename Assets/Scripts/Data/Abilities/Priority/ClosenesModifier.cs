using UnityEngine;

[System.Serializable]
public class CloseEnemyModifier : PriorityModifier
{
    [field: SerializeField] private float Distance { get; set; }

    public override int AlterPriority(PawnController abilityUser, Battle battle, int priority)
    {
        var selectedPawn = battle.QueryEnemies(abilityUser, FocusType.Closest, 0);

        if (selectedPawn != null && (abilityUser.transform.position - selectedPawn.transform.position).magnitude < Distance)
            return priority + 1 * Multiplier;

        return priority;
    }
}
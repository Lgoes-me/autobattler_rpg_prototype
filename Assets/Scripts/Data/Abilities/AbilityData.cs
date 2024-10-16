using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject
{
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private float Delay { get; set; }
    [field: SerializeField] private float Range { get; set; }
    [field: SerializeField] private AbilityBehaviourData[] AbilityBehaviours { get; set; }
    [field: SerializeField] [field: SerializeReference] public BaseResourceData ResourceData { get; private set; }
    [field: SerializeField] [field: SerializeReference] private PriorityModifier[] Priorities { get; set; }

    public Ability ToDomain(PawnController abilityUser)
    {
        var resourceComponent = ResourceData.ToDomain(abilityUser);
        var abilityBehaviours = AbilityBehaviours.Select(a => a.ToDomain(abilityUser)).ToList();
        
        return new Ability(
            abilityUser, 
            Animation,
            Delay, 
            Range,
            abilityBehaviours,
            resourceComponent); 
    }

    public int GetPriority(PawnController abilityUser, Battle battle)
    {
        var priority = 1;
        
        foreach (var priorityModifier in Priorities)
        {
            priority = priorityModifier.AlterPriority(abilityUser, battle, AbilityBehaviours[0].FocusData, priority);
        }
        
        return priority;
    }
}
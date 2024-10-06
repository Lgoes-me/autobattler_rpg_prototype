using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject
{
    [field: SerializeField] [field: SerializeReference] private BaseEffectData EffectData { get; set; }
    [field: SerializeField] [field: SerializeReference] private BaseActionData ActionData { get; set; }
    
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private float Range { get; set; }
    [field: SerializeField] private float Delay { get; set; }
    [field: SerializeField] private TargetType Target { get; set; }
    [field: SerializeField] public int NumberOfTargets { get; set; }
    
    [field: SerializeField] private FocusType Focus { get; set; }
    [field: SerializeField] private int Error { get; set; }
    [field: SerializeField] public int ManaCost { get; set; }

    public Ability ToDomain(PawnController abilityUser)
    {
        var effect = EffectData.ToDomain();
        var focusComponent = new AbilityFocusComponent(abilityUser, Target, Focus, Error);
        var resourceComponent = new AbilityResourceComponent(abilityUser, ManaCost);
        var actionComponent = ActionData.ToDomain(abilityUser, focusComponent, effect);
        
        return new Ability(
            abilityUser, 
            Animation,
            Range, 
            Delay, 
            effect, 
            focusComponent, 
            resourceComponent,
            actionComponent); 
    }

    public int GetPriority()
    {
        return 1;
    }
}
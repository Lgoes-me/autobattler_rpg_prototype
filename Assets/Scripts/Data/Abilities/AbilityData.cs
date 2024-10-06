using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject
{
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private float Range { get; set; }
    [field: SerializeField] private float Delay { get; set; }
    [field: SerializeField] [field: SerializeReference] private BaseEffectData EffectData { get; set; }
    [field: SerializeField] [field: SerializeReference] private BaseFocusData FocusData { get; set; }
    [field: SerializeField] [field: SerializeReference] private BaseResourceData ResourceData { get; set; }
    [field: SerializeField] [field: SerializeReference] private BaseActionData ActionData { get; set; }

    public Ability ToDomain(PawnController abilityUser)
    {
        var effect = EffectData.ToDomain();
        var focusComponent = FocusData.ToDomain(abilityUser);
        var resourceComponent = ResourceData.ToDomain(abilityUser);
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
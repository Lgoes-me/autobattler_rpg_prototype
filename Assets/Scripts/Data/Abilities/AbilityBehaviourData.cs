using UnityEngine;

[System.Serializable]
public class AbilityBehaviourData
{
    [field: SerializeField] [field: SerializeReference] private BaseEffectData EffectData { get; set; }
    [field: SerializeField] [field: SerializeReference] public BaseFocusData FocusData { get; private set; }
    [field: SerializeField] [field: SerializeReference] private BaseActionData ActionData { get; set; }
    
    public AbilityBehaviour ToDomain(PawnController abilityUser)
    {
        var effect = EffectData.ToDomain(abilityUser);
        var focusComponent = FocusData.ToDomain(abilityUser);
        var actionComponent = ActionData.ToDomain(abilityUser, effect);
        return new AbilityBehaviour(effect, focusComponent, actionComponent);
    }
}
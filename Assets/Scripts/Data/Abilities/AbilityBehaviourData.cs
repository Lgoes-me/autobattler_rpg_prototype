using System.Linq;
using UnityEngine;

[System.Serializable]
public class AbilityBehaviourData
{
    [field: SerializeField] [field: SerializeReference] private BaseEffectData[] Effects { get; set; }
    [field: SerializeField] [field: SerializeReference] public BaseFocusData FocusData { get; private set; }
    [field: SerializeField] [field: SerializeReference] private BaseActionData ActionData { get; set; }
    
    public AbilityBehaviour ToDomain(PawnController abilityUser)
    {
        var focusComponent = FocusData.ToDomain(abilityUser);
        var effects = Effects.Select(e => e.ToDomain(abilityUser)).ToList();
        var actionComponent = ActionData.ToDomain(abilityUser, effects, focusComponent);
        return new AbilityBehaviour(effects, focusComponent, actionComponent);
    }
}
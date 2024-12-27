using System.Linq;
using UnityEngine;

[System.Serializable]
public class AbilityBehaviourData
{
    [field: SerializeField] [field: SerializeReference] private EffectData[] Effects { get; set; }
    [field: SerializeField] [field: SerializeReference] public FocusData FocusData { get; private set; }
    [field: SerializeField] [field: SerializeReference] private ActionData ActionData { get; set; }
    
    public AbilityBehaviour ToDomain(PawnController abilityUser)
    {
        var focusComponent = FocusData?.ToDomain();
        var effects = Effects.Select(e => e.ToDomain(abilityUser)).ToList();
        var actionComponent = ActionData.ToDomain(abilityUser, effects, focusComponent);
        return new AbilityBehaviour(effects, focusComponent, actionComponent);
    }
}
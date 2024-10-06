using UnityEngine;

[System.Serializable]
public class FocusComponent : BaseFocusData
{
    [field: SerializeField] private TargetType Target { get; set; }
    [field: SerializeField] private FocusType Focus { get; set; }
    [field: SerializeField] public int NumberOfTargets { get; set; }
    [field: SerializeField] private int Error { get; set; }
    
    public override AbilityFocusComponent ToDomain(PawnController abilityUser)
    {
        return new AbilityFocusComponent(abilityUser, Target, Focus, NumberOfTargets, Error);
    }
}
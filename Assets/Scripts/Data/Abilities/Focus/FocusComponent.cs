using UnityEngine;

[System.Serializable]
public class FocusComponent : BaseFocusData
{
    [field: SerializeField] private TargetType Target { get; set; }
    [field: SerializeField] private FocusType Focus { get; set; }
    [field: SerializeField] private float Range { get; set; }
    [field: SerializeField] private int Error { get; set; }
    
    public override AbilityFocusComponent ToDomain(PawnController abilityUser)
    {
        return new AbilityFocusComponent(abilityUser, Target, Focus, Range, Error);
    }
}
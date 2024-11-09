using UnityEngine;

[System.Serializable]
public class FocusComponentData : FocusData
{
    [field: SerializeField] private int Error { get; set; }
    
    public override AbilityFocusComponent ToDomain(PawnController abilityUser)
    {
        return new AbilityFocusComponent(abilityUser, Target, Focus, Error);
    }
}

using UnityEngine;

[System.Serializable]
public class AllyFocusComponentData : FocusData
{
    [field: SerializeField] private bool CanTargetSelf { get; set; }
    
    public override AbilityFocusComponent ToDomain()
    {
        return new AllyFocusComponent(CanTargetSelf);
    }
}

using UnityEngine;

[System.Serializable]
public class AllyFocusComponentData : FocusData
{
    [field: SerializeField] public FocusType Focus { get; protected set; }
    [field: SerializeField] private bool CanTargetSelf { get; set; }
    
    public override AbilityFocusComponent ToDomain()
    {
        return new AllyFocusComponent(Focus, CanTargetSelf);
    }
}
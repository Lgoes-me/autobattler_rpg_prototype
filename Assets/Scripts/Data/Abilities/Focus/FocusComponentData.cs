using UnityEngine;

[System.Serializable]
public class FocusComponentData : FocusData
{
    [field: SerializeField] private int Error { get; set; }
    
    public override AbilityFocusComponent ToDomain()
    {
        return new AbilityFocusComponent(Target, Focus, Error);
    }
}
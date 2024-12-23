using UnityEngine;

[System.Serializable]
public abstract class FocusData : IComponentData
{
    [field: SerializeField] public TargetType Target { get; protected set; }
    [field: SerializeField] public FocusType Focus { get; protected set; }
    
    public abstract AbilityFocusComponent ToDomain();
}
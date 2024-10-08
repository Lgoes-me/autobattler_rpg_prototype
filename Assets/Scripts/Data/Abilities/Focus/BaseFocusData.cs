using UnityEngine;

[System.Serializable]
public abstract class BaseFocusData : BaseComponentData
{
    [field: SerializeField] public TargetType Target { get; protected set; }
    [field: SerializeField] public FocusType Focus { get; protected set; }
    [field: SerializeField] public float Range { get; protected set; }
    
    public abstract AbilityFocusComponent ToDomain(PawnController abilityUser);
}
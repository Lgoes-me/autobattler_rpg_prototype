using UnityEngine;

[System.Serializable]
public class AnimationState
{
    [field: SerializeField] public virtual string Animation { get; set; }
    [field: SerializeField] public virtual bool Loopable { get; set; } = false;
    [field: SerializeField] public virtual bool CanTakeTurn { get; set; } = false;
    [field: SerializeField] public virtual bool CanBeTargeted { get; set; } = true;
    [field: SerializeField] public virtual bool AbleToFight { get; set; } = true;
}
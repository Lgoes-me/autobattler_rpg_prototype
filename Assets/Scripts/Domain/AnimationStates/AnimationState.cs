﻿using UnityEngine;

[System.Serializable]
public class AnimationState
{
    [field: SerializeField] public virtual string Animation { get; set; }
    [field: SerializeField] public virtual bool CanWalk { get; set; } = false;
    [field: SerializeField] public virtual bool CanBeTargeted { get; set; } = true;
    [field: SerializeField] public virtual bool AbleToFight { get; set; } = true;

    public virtual void DoAnimationEvent()
    {
        
    }
    
    
    public virtual void DoAnimationCallback()
    {
        
    }
}
using UnityEngine;

[System.Serializable]
public class AnimationState
{
    [field : SerializeField] public string Animation { get; set; }
    [field : SerializeField] public bool CanAttack { get; set; }
}

public class IdleState : AnimationState
{
    public new string Animation => "Idle";
}
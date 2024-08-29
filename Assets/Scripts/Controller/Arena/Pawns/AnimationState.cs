using UnityEngine;

[System.Serializable]
public class AnimationState
{
    [field: SerializeField] public virtual string Animation { get; }
    [field: SerializeField] public virtual bool Loopable { get; set; } = false;
    [field: SerializeField] public virtual bool CanTakeTurn { get; set; } = false;
    [field: SerializeField] public virtual bool CanBeTargeted { get; set; } = true;
    [field: SerializeField] public virtual bool AbleToFight { get; set; } = true;
}

public class IdleState : AnimationState
{
    public override string Animation => "Idle";
    public override bool Loopable => true;
    public override bool CanTakeTurn => true;
}

public class AttackState : AnimationState
{
    public override string Animation => "Attack";
}

public class DeadState : AnimationState
{
    public override string Animation => "Dead";
    public override bool CanBeTargeted => false;
    public override bool AbleToFight => false;
}
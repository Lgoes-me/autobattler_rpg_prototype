public class AttackState : AnimationState
{
    public override string Animation => Attack.Animation;
    private Attack Attack { get; }

    public AttackState(Attack attack)
    {
        Attack = attack;
    }
}
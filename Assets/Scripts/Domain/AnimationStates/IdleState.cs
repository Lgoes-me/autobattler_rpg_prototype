﻿public class IdleState : AnimationState
{
    public override string Animation => "Idle";
    public override bool CanWalk => true;
    public override bool Loopable => true;
    public override bool CanTakeTurn => true;
}
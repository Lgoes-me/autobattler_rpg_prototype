using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityFocusComponent
{
    private PawnController AbilityUser { get; set; }
    private TargetType Target { get; set; }
    private FocusType Focus { get; set; }
    private float Range { get; set; }
    private int Error { get; set; }

    public bool IsInRange => Range >= (FocusedPawnPosition - AbilityUser.transform.position).magnitude;
    public Vector3 FocusedPawnPosition => FocusedPawn != null ? FocusedPawn.transform.position : AbilityUser.transform.position;
    public Vector3 WalkingDestination =>
        FocusedPawn != null ? 
            FocusedPawn.transform.position + Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * (Range - 1)
            : AbilityUser.transform.position;
    
    public PawnController FocusedPawn { get; private set; }

    public AbilityFocusComponent(PawnController abilityUser, TargetType target, FocusType focus, float range, int error)
    {
        AbilityUser = abilityUser;
        Target = target;
        Focus = focus;
        Range = range;
        Error = error;
        FocusedPawn = null;
    }

    public void ChooseFocus(Battle battle)
    {
        FocusedPawn = battle.Query(AbilityUser, Target, Focus, Error);
    }
}

public enum TargetType
{
    Self = 0,
    Ally = 1,
    Enemy = 2
}

public enum FocusType
{
    Unknown = 0,
    Closest = 1,
    Farthest = 2,
    LowestLife = 3,
    HighestLife = 4
}
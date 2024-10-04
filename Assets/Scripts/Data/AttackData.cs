using UnityEngine;

[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private Damage Damage { get; set; }
    [field: SerializeField] private int Range { get; set; }
    [field: SerializeField] private float Delay { get; set; }
    [field: SerializeField] private TargetType Target { get; set; }
    [field: SerializeField] public int NumberOfTargets { get; set; }
    [field: SerializeField] private FocusType Focus { get; set; }
    [field: SerializeField] private int Error { get; set; }
    [field: SerializeField] public int ManaCost { get; set; }
    [field: SerializeField] private ProjectileController Projectile { get; set; }

    public Ability ToDomain(PawnController abilityUser)
    {
        return new Ability(abilityUser, Animation, Damage, Range, Delay, Target, Focus, Error, ManaCost, Projectile); 
    }

    public int GetPriority()
    {
        return 1;
    }
}
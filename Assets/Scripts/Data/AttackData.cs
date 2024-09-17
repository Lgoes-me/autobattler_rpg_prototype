using UnityEngine;

[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [field: SerializeField] private string Animation { get; set; }
    [field: SerializeField] private Damage Damage { get; set; }
    [field: SerializeField] private int Range { get; set; }
    [field: SerializeField] private float Delay { get; set; }
    [field: SerializeField] private TargetType Target { get; set; }
    [field: SerializeField] private FocusType Focus { get; set; }
    [field: SerializeField] private int Error { get; set; }
    [field: SerializeField] public int ManaCost { get; set; }
    [field: SerializeField] private ProjectileController Projectile { get; set; }

    public Attack ToDomain(PawnController pawnController)
    {
        return new Attack(pawnController, Animation, Damage, Range, Delay, Target, Focus, Error, ManaCost, Projectile);
    }
}

[System.Serializable]
public class Damage
{
    [field: SerializeField] public int Value { get; set; }
    [field: SerializeField] public DamageType Type { get; set; }
}

public enum DamageType
{
    Heal = 0,
    Slash = 1,
    Magical = 2,
    Fire = 3,
    Buff = 4,
    Debug = 5
}

public enum TargetType
{
    Unknown = 0,
    Ally = 1,
    Enemy = 2
}

public enum FocusType
{
    Self = 0,
    Closest = 1,
    Farthest = 2,
    LowestLife = 3,
    HighestLife = 4
}
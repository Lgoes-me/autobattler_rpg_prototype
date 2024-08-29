using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] private int Health { get; set; } = 10;
    [field: SerializeField] private int Attack { get; set; } = 2;
    [field: SerializeField] private int AttackRange { get; set; } = 2;
    [field: SerializeField] private int Size { get; set; } = 1;
    [field: SerializeField] private int Initiative { get; set; } = 1;

    public PawnDomain ToDomain()
    {
        return new PawnDomain(Health, Attack, AttackRange, Size, Initiative);
    }
}
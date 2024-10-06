using UnityEngine;

public class DamageEffect : AbilityEffect
{
    private Damage Damage { get; set; }

    public DamageEffect(Damage damage)
    {
        Damage = damage;
    }

    public override void DoAbilityEffect(PawnController pawnController)
    {
        var pawn = pawnController.Pawn;
        
        if(pawn.Health == 0)
            return;
        
        pawn.Health = Mathf.Clamp(pawn.Health - Damage.Value, 0, pawn.MaxHealth);
        
        pawnController.ReceiveAttack();
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
    Slash = 1,
    Magical = 2,
    Fire = 3
}

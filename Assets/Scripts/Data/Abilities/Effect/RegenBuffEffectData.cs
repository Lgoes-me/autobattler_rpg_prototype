using UnityEngine;

public class RegenBuffEffectData: BaseEffectData
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private int Regen { get; set; }
    [field: SerializeField] private int Variation { get; set; }
    [field: SerializeField]  private float Duration { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var regen = Regen * abilityUser.Pawn.Stats.Arcane;
        var regenBuff = new RegenBuff(regen, Variation, Id, Duration);
        return new RegenBuffEffect(abilityUser, regenBuff);
    }
}
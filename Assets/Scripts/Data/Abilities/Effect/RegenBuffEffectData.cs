using UnityEngine;

public class RegenBuffEffectData: BaseEffectData
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private int Regen { get; set; }
    [field: SerializeField] private int TickRate { get; set; }
    [field: SerializeField]  private float Duration { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var regen = Regen;// * abilityUser.Pawn.Stats.Strength;
        var regenBuff = new RegenBuff(regen, TickRate, Id, Duration);
        return new RegenBuffEffect(abilityUser, regenBuff);
    }
}
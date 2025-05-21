using UnityEngine;

public class BuffEffectData : EffectData
{
    [field: SerializeField] private BuffData BuffData { get; set; }
    [field: SerializeField] private float Duration { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var buff = BuffData.ToDomain(abilityUser.Pawn, Duration);
        return new BuffEffect(abilityUser, buff);
    }
}
using UnityEngine;

public class BuffEffectData : EffectData
{
    [field: SerializeField] private BuffData BuffData { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        var buff = BuffData.ToDomain(abilityUser);
        return new BuffEffect(abilityUser, buff);
    }
}
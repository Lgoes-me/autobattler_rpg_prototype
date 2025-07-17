using UnityEngine;

[System.Serializable]
public class GiveManaEffectData : EffectData
{
    [field: SerializeField] private int ManaValue { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new GiveManaEffect(abilityUser, ManaValue);
    }
}

[System.Serializable]
public class GivePercentManaEffectData : EffectData
{
    [field: Range(0,1f)] [field: SerializeField] private float ManaPercent { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new GivePercentManaEffect(abilityUser, ManaPercent);
    }
}

[System.Serializable]
public class RemoveManaEffectData : EffectData
{
    [field: SerializeField] private int ManaValue { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new RemoveManaEffect(abilityUser, ManaValue);
    }
}

[System.Serializable]
public class RemovePercentManaEffectData : EffectData
{
    [field: Range(0,1f)] [field: SerializeField] private float ManaPercent { get; set; }

    public override AbilityEffect ToDomain(PawnController abilityUser)
    {
        return new RemovePercentManaEffect(abilityUser, ManaPercent);
    }
}
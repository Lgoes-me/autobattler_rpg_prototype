[System.Serializable]
public abstract class EffectData : IComponentData
{
    public abstract AbilityEffect ToDomain(PawnController abilityUser);
}
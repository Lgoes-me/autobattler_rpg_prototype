[System.Serializable]
public abstract class BaseEffectData : BaseComponentData
{
    public abstract AbilityEffect ToDomain(PawnController abilityUser);
}
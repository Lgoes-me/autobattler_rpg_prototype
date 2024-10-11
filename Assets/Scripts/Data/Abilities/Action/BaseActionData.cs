[System.Serializable]
public abstract class BaseActionData : BaseComponentData
{
    public abstract AbilityActionComponent ToDomain(PawnController abilityUser, AbilityEffect effect);
}
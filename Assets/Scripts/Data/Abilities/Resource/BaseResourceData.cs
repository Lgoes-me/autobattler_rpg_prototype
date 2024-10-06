[System.Serializable]
public abstract class BaseResourceData : AbilityComponentData
{
    public abstract AbilityResourceComponent ToDomain(PawnController abilityUser);
}
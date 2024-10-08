[System.Serializable]
public abstract class BaseResourceData : BaseComponentData
{
    public abstract AbilityResourceComponent ToDomain(PawnController abilityUser);
    public abstract int GetCost();
}
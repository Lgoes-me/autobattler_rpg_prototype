
[System.Serializable]
public class NoResourceData : BaseResourceData
{
    public override AbilityResourceComponent ToDomain(PawnController abilityUser)
    {
        return new NoResourceComponent(abilityUser);
    }
}
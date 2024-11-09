
[System.Serializable]
public class NoResourceData : ResourceData
{
    public override AbilityResourceComponent ToDomain(PawnController abilityUser)
    {
        return new NoResourceComponent(abilityUser);
    }

    public override int GetCost() => 0;
}
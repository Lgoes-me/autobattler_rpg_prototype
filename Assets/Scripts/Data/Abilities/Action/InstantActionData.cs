[System.Serializable]
public class InstantActionData : BaseActionData
{
    public override AbilityActionComponent ToDomain(PawnController abilityUser, AbilityEffect effect)
    {
        return new InstantActionComponent(abilityUser, effect);
    }
}
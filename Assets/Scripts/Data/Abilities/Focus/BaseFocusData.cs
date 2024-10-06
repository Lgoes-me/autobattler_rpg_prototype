[System.Serializable]
public abstract class BaseFocusData : AbilityComponentData
{
    public abstract AbilityFocusComponent ToDomain(PawnController abilityUser);
}
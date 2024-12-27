
[System.Serializable]
public class SelfFocusComponentData : FocusData
{
    public override AbilityFocusComponent ToDomain()
    {
        return new SelfFocusComponent();
    }
}
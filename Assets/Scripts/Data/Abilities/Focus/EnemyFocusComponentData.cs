[System.Serializable]
public class EnemyFocusComponentData : FocusData
{
    public override AbilityFocusComponent ToDomain()
    {
        return new EnemyFocusComponent();
    }
}
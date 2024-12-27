using UnityEngine;

[System.Serializable]
public class EnemyFocusComponentData : FocusData
{
    [field: SerializeField] private int Error { get; set; }
    
    public override AbilityFocusComponent ToDomain()
    {
        return new EnemyFocusComponent(Error);
    }
}
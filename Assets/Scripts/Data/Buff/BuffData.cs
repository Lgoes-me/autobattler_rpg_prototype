using UnityEngine;

[CreateAssetMenu]
public class BuffData : ScriptableObject
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] [field: SerializeReference] private BuffComponentData BuffComponentData { get; set; }

    public Buff ToDomain(PawnController focus)
    {
        return BuffComponentData.ToDomain(Id, focus);
    }
    
}

public abstract class BuffComponentData : IComponentData
{
    public abstract Buff ToDomain(string id, PawnController abilityUser);
}
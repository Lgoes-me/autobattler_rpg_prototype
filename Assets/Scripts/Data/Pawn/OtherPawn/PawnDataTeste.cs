using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class PawnDataTeste : ScriptableObject
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<PawnDataComponent> Components { get; set; }

    public TestePawn ToDomain()
    {
        var components = Components.Select(c => c.ToDomain()).ToList();
        return new TestePawn(Id, components);
    }

    private void OnValidate()
    {
        if (Id != string.Empty)
            return;

        Id = name;
    }
}

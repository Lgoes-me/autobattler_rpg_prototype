using System.Linq;
using TMPro;
using UnityEngine;

public class ArchetypeCanvasController : BaseCanvasHolderItemController<ArchetypeData>
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private TextMeshProUGUI Quantidade { get; set; }
    
    public override BaseCanvasHolderItemController<ArchetypeData> Init(ArchetypeData archetype)
    {
        Name.SetText(archetype.name);

        var currentNumberOfPawns = archetype.CurrentArchetype?.Pawns ?? 0;
        var nextNumberOfPawns = archetype.NextArchetype?.Pawns ?? archetype.CurrentArchetype?.Pawns ?? archetype.Archetypes.First().Pawns;
        
        Quantidade.SetText($"{currentNumberOfPawns} / {nextNumberOfPawns}");
        
        Show();
        
        return this;
    }
}
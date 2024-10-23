using TMPro;
using UnityEngine;

public class ArchetypeCanvasController : BaseCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private TextMeshProUGUI Quantidade { get; set; }
    
    public ArchetypeCanvasController Init(Archetype archetype)
    {
        Name.SetText(archetype.Identifier.ToString());
        Quantidade.SetText(archetype.Quantidade.ToString());
        Show();
        
        return this;
    }
}
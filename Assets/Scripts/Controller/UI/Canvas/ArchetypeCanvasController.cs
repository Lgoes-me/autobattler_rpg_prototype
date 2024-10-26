using System.Linq;
using TMPro;
using UnityEngine;

public class ArchetypeCanvasController : BaseCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private TextMeshProUGUI Quantidade { get; set; }
    
    public ArchetypeCanvasController Init(Archetype archetype)
    {
        Name.SetText(archetype.Identifier.ToString());

        var nextStep = archetype.CurrentAmount;
        
        foreach (var step in archetype.AmountSteps)
        {
            if (step < archetype.CurrentAmount) 
                continue;
            
            nextStep = step;
            break;
        }

        Quantidade.SetText($"{archetype.CurrentAmount} / {nextStep}");
        
        Show();
        
        return this;
    }
}
using TMPro;
using UnityEngine;

public class ArchetypeCanvasController : BaseCanvasHolderItemController<ArchetypeData>
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private TextMeshProUGUI Quantidade { get; set; }
    
    public override BaseCanvasHolderItemController<ArchetypeData> Init(ArchetypeData archetype)
    {
        Name.SetText(archetype.name);

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
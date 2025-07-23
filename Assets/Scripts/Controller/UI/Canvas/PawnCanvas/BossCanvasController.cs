using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossCanvasController : LifeBarCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private BossModifierCanvasHolderController BossModifierCanvasHolderController { get; set; }
    
    public override void Init(Pawn pawn)
    {
        base.Init(pawn);
        Name.SetText(Pawn.Id);

        foreach (var ability in Pawn.GetComponent<AbilitiesComponent>().Abilities)
        {
            if (ability.ResourceData is NoResourceData)
                continue;
            
            Debug.Log(ability.Id);
        }
    }

    public void SetModifiers(List<BossModifierData> modifiers)
    {
        BossModifierCanvasHolderController.UpdateItems(modifiers);
    }
}
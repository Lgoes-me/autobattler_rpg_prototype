using TMPro;
using UnityEngine;

public class BossModifierCanvasController : BaseCanvasHolderItemController<BossModifierData>
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override BaseCanvasHolderItemController<BossModifierData> Init(BossModifierData bossModifier)
    {
        Name.SetText(bossModifier.name);
        Show();

        return this;
    }
}
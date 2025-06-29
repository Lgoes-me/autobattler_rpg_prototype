using TMPro;
using UnityEngine;

public class BossModifierCanvasController : BaseCanvasHolderItemController<BossModifier>
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override BaseCanvasHolderItemController<BossModifier> Init(BossModifier bossModifier)
    {
        Name.SetText(bossModifier.Identifier.ToString());
        Show();

        return this;
    }
}
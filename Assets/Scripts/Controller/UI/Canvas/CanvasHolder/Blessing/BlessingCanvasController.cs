using TMPro;
using UnityEngine;

public class BlessingCanvasController : BaseCanvasHolderItemController<Blessing>
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override BaseCanvasHolderItemController<Blessing> Init(Blessing blessing)
    {
        Name.SetText(blessing.Identifier.ToString());
        Show();

        return this;
    }
}
using TMPro;
using UnityEngine;

public class BlessingCanvasController : BaseCanvasHolderItemController<BlessingData>
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override BaseCanvasHolderItemController<BlessingData> Init(BlessingData blessing)
    {
        Name.SetText(blessing.name.ToString());
        Show();

        return this;
    }
}
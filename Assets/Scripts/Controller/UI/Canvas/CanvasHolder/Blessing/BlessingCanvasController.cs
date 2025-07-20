using TMPro;
using UnityEngine;

public class BlessingCanvasController : BaseCanvasHolderItemController<BlessingData>
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override BaseCanvasHolderItemController<BlessingData> Init(BlessingData blessing)
    {
        Name.SetText(blessing.Id.ToString());
        Show();

        return this;
    }
}
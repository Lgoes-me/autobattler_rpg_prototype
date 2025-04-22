using TMPro;
using UnityEngine;

public class BlessingCanvasController : BaseCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public void Init(Blessing blessing)
    {
        Name.SetText(blessing.Identifier.ToString());
        Show();
    }
}
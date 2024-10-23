using TMPro;
using UnityEngine;

public class BlessingCanvasController : BaseCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public void Init(BlessingIdentifier blessing)
    {
        Name.SetText(blessing.ToString());
        Show();
    }
}
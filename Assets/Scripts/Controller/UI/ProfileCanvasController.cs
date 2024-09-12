using TMPro;
using UnityEngine;

public class ProfileCanvasController : BaseCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private PawnCanvasController PawnCanvasController { get; set; }

    public override void Show()
    {
        Name.SetText(PawnCanvasController.PawnController.PawnData.name);
        base.Show();
    }
}
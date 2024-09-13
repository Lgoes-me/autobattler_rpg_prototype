using TMPro;
using UnityEngine;

public class ProfileCanvasController : PawnCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }

    public override void Init(PawnController pawnController)
    {
        base.Init(pawnController);
        Name.SetText(pawnController.PawnData.name);
    }
}
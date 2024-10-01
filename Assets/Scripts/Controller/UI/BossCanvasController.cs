using TMPro;
using UnityEngine;

public class BossCanvasController : PawnCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override void Init(PawnController pawnController)
    {
        base.Init(pawnController);
        Name.SetText(pawnController.PawnData.name);
    }
}
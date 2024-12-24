using TMPro;
using UnityEngine;

public class BossCanvasController : LifeBarCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override void Init(PawnController pawnController)
    {
        base.Init(pawnController);
        Name.SetText(Pawn.Id);
    }
}
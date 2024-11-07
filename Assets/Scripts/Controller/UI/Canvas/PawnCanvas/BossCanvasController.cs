using TMPro;
using UnityEngine;

public class BossCanvasController : PawnCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override void Init(Pawn pawn)
    {
        base.Init(pawn);
        Name.SetText(Pawn.Id);
    }
}
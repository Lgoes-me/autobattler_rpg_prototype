using TMPro;
using UnityEngine;

public class BossCanvasController : PawnCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override void Init(PawnDomain pawn)
    {
        base.Init(pawn);
        Name.SetText(Pawn.Id);
    }
}
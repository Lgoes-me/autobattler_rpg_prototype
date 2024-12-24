using TMPro;
using UnityEngine;

public class BossCanvasController : LifeBarCanvasController
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    public override void Init(Pawn pawn)
    {
        base.Init(pawn);
        Name.SetText(Pawn.Id);
    }
}
using UnityEngine;

public class ProfileCanvasItemController: BaseCanvasHolderItemController<PawnController>
{
    [field: SerializeField] private ProfileCanvasController Profile { get; set; }
    
    public override BaseCanvasHolderItemController<PawnController> Init(PawnController pawnController)
    {
        Profile.Init(pawnController.Pawn);
        return this;
    }
}
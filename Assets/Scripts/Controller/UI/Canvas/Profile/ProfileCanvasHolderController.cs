using System.Collections.Generic;
using UnityEngine;

public class ProfileCanvasHolderController : MonoBehaviour
{
    [field: SerializeField] private List<LifeBarCanvasController> PawnCanvases { get; set; }
    
    private void Awake()
    {
        foreach (var pawnCanvas in PawnCanvases)
        {
            pawnCanvas.Hide();
        }
    }
    
    public void UpdateProfileCanvas(List<PawnController> playerPawns)
    {
        foreach (var pawnCanvas in PawnCanvases)
        {
            pawnCanvas.Hide();
        }

        for (var index = 0; index < PawnCanvases.Count && index < playerPawns.Count; index++)
        {
            var pawnCanvas = PawnCanvases[index];
            var playerPawn = playerPawns[index].Pawn;
            
            pawnCanvas.Init(playerPawn);
        }
    }

}
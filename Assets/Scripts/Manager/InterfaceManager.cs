using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [field: SerializeField] private GameObject BattleCanvas { get; set; }
    [field: SerializeField] private GameObject BattleLostCanvas { get; set; }
    [field: SerializeField] private List<PawnCanvasController> PawnCanvases { get; set; }
    
    [field: SerializeField] public BossCanvasController BossCanvas { get; private set; }
    
    public void InitProfileCanvas(List<PawnController> playerPawns)
    {
        for (var index = 0; index < PawnCanvases.Count && index < playerPawns.Count; index++)
        {
            var pawnCanvas = PawnCanvases[index];
            var playerPawn = playerPawns[index];

            playerPawn.PawnCanvasController = pawnCanvas;
            pawnCanvas.Init(playerPawn.Pawn);
        }
    }

    public void InitBlessingsCanvas(List<Blessing> blessings)
    {
        
    }

    public void InitArchetypeCanvas(List<Archetype> archetypes)
    {
        
    }
    
    public void ShowDefeatCanvas()
    {
        BattleLostCanvas.SetActive(true);
    }

    public void HideDefeatCanvas()
    {
        BattleLostCanvas.SetActive(false);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [field: SerializeField] private GameObject BattleCanvas { get; set; }
    [field: SerializeField] private GameObject BattleLostCanvas { get; set; }
    
    [field: SerializeField] private List<BlessingCanvasController> BlessingCanvases { get; set; }
    [field: SerializeField] private List<PawnCanvasController> PawnCanvases { get; set; }
    [field: SerializeField] private RectTransform ArchetypeCanvasParent { get; set; }
    [field: SerializeField] private ArchetypeCanvasController ArchetypeCanvasControllerPrefab { get; set; }
    [field: SerializeField] public BossCanvasController BossCanvas { get; private set; }

    private List<ArchetypeCanvasController> ArchetypeCanvases { get; set; }

    private void Awake()
    {
        foreach (var pawnCanvas in PawnCanvases)
        {
            pawnCanvas.Hide();
        }

        foreach (var blessingCanvas in BlessingCanvases)
        {
            blessingCanvas.Hide();
        }

        ArchetypeCanvases = new List<ArchetypeCanvasController>();
    }

    public void InitProfileCanvas(List<PawnController> playerPawns)
    {
        foreach (var pawnCanvas in PawnCanvases)
        {
            pawnCanvas.Hide();
        }

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
        foreach (var blessingCanvas in BlessingCanvases)
        {
            blessingCanvas.Hide();
        }
        
        for (var index = 0; index < BlessingCanvases.Count && index < blessings.Count; index++)
        {
            var blessingCanvas = BlessingCanvases[index];
            var blessing = blessings[index];

            blessingCanvas.Init(blessing);
        }
    }

    public void InitArchetypesCanvas(List<Archetype> archetypes)
    {
        foreach (var archetypeCanvas in ArchetypeCanvases)
        {
            Destroy(archetypeCanvas.gameObject);
        }

        ArchetypeCanvases.Clear();

        foreach (var archetype in archetypes)
        {
            var archetypeCanvasController =
                Instantiate(ArchetypeCanvasControllerPrefab, ArchetypeCanvasParent).Init(archetype);
            ArchetypeCanvases.Add(archetypeCanvasController);
        }
    }

    public void ShowDefeatCanvas()
    {
        HideBattleCanvas();
        BattleLostCanvas.SetActive(true);
    }

    public void HideDefeatCanvas()
    {
        ShowBattleCanvas();
        BattleCanvas.SetActive(true);
    }

    public void HideBattleCanvas()
    {
        BattleCanvas.SetActive(false);
    }
    
    public void ShowBattleCanvas()
    {
        BattleCanvas.SetActive(true);
    }
    
}
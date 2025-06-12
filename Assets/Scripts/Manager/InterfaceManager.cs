using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InterfaceManager : MonoBehaviour, IManager
{
    [field: SerializeField] private GameObject BattleCanvas { get; set; }
    [field: SerializeField] private GameObject BattleLostCanvas { get; set; }
    [field: SerializeField] private GameObject DungeonVictoryCanvas { get; set; }
    [field: SerializeField] public BossCanvasController BossCanvas { get; private set; }

    [field: SerializeField] private PrizeOptionHolderController PrizeOptionHolderController { get; set; }
    [field: SerializeField] private BlessingCanvasHolderController BlessingCanvasHolderController { get; set; }
    [field: SerializeField] private ProfileCanvasHolderController ProfileCanvasHolderController { get; set; }
    [field: SerializeField] private ArchetypeCanvasHolderController ArchetypeCanvasHolderController { get; set; }

    public void UpdateProfileCanvas(List<PawnController> playerPawns)
    {
        ProfileCanvasHolderController.UpdateItems(playerPawns);
    }

    public void UpdateBlessingsCanvas(List<Blessing> blessings)
    {
        BlessingCanvasHolderController.UpdateItems(blessings);
    }

    public void UpdateArchetypesCanvas(List<Archetype> archetypes)
    {
        ArchetypeCanvasHolderController.UpdateItems(archetypes);
    }
    
    public Task<T> ShowPrizeCanvas<T>(BasePrize<T> prize) where T : BasePrizeItem
    {
        return PrizeOptionHolderController.ShowPrizeCanvas(prize);
    }

    public void ShowDefeatCanvas()
    {
        HideBattleCanvas();
        BattleLostCanvas.SetActive(true);
    }

    public void HideDefeatCanvas()
    {
        BattleLostCanvas.SetActive(false);
        ShowBattleCanvas();
    }

    public void HideBattleCanvas()
    {
        BattleCanvas.SetActive(false);
    }
    
    public void ShowBattleCanvas()
    {
        BattleCanvas.SetActive(true);
    }
    
    public void ShowDungeonVictoryCanvas(Action callback)
    {
        HideBattleCanvas();
        DungeonVictoryCanvas.SetActive(true);
        StartCoroutine(WaitVictoryCanvas(callback));
    }

    private IEnumerator WaitVictoryCanvas(Action callback)
    {
        yield return new WaitForSeconds(3f);
        HideDungeonVictoryCanvas();
        callback?.Invoke();
    }
    
    public void HideDungeonVictoryCanvas()
    {
        DungeonVictoryCanvas.SetActive(false);
    }
}
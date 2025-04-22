using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InterfaceManager : MonoBehaviour, IManager
{
    [field: SerializeField] private GameObject BattleCanvas { get; set; }
    [field: SerializeField] private GameObject BattleLostCanvas { get; set; }
    [field: SerializeField] public BossCanvasController BossCanvas { get; private set; }

    [field: SerializeField] private PrizeOptionHolderController PrizeOptionHolderController { get; set; }
    [field: SerializeField] private BlessingCanvasHolderController BlessingCanvasHolderController { get; set; }
    [field: SerializeField] private ProfileCanvasHolderController ProfileCanvasHolderController { get; set; }
    [field: SerializeField] private ArchetypeCanvasHolderController ArchetypeCanvasHolderController { get; set; }
    [field: SerializeField] private ConsumableCanvasHolderController ConsumableCanvasHolderController { get; set; }

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

    public void UpdateConsumablesCanvas(List<ConsumableData> consumables)
    {
        ConsumableCanvasHolderController.UpdateItems(consumables);
    }

    public Task<T> ShowPrizeCanvas<T>(BasePrize<T> prize) where T : BasePrizeItem
    {
        return PrizeOptionHolderController.ShowPrizeCanvas(prize);
    }

    public void StartBattle()
    {
        ConsumableCanvasHolderController.StartBattle();
    }

    public void FinishBattle()
    {
        ConsumableCanvasHolderController.FinishBattle();
    }
    
    public void ShowDefeatCanvas()
    {
        HideBattleCanvas();
        BattleLostCanvas.SetActive(true);
    }

    public void HideDefeatCanvas()
    {
        ShowBattleCanvas();
        BattleLostCanvas.SetActive(false);
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
using System.Collections.Generic;
using UnityEngine;

public class PrizeManager : MonoBehaviour
{
    [field: SerializeField] private InterfaceManager InterfaceManager { get; set; }
    [field: SerializeField] private PauseManager PauseManager { get; set; }

    public async void CreateLevelUpPrize()
    {
        PauseManager.PauseGame();
        
        var gameSaveManager = Application.Instance.GameSaveManager;
        var prizes = new LevelUpPrize(3, gameSaveManager.GetSelectedParty());

        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        PauseManager.ResumeGame();
        selectedPrize.LevelUp();
        gameSaveManager.SavePawnInfo(selectedPrize);
    }

    public async void CreateBlessingPrize(List<BlessingIdentifier> blessings)
    {
        PauseManager.PauseGame();
        var prizes = new BlessingPrize(3, blessings);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        PauseManager.ResumeGame();
        Application.Instance.BlessingManager.AddBlessing(selectedPrize);
    }
}

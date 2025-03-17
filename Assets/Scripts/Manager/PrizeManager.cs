using UnityEngine;

public class PrizeManager : MonoBehaviour
{
    [field: SerializeField] private InterfaceManager InterfaceManager { get; set; }

    public async void CreateLevelUpPrize()
    {
        var gameSaveManager = Application.Instance.GameSaveManager;
        var prizes = new LevelUpPrize(3, gameSaveManager.GetSelectedParty());

        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
    }

    public async void CreateBlessingPrize()
    {
        var prizes = new BlessingPrize(3);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        Application.Instance.BlessingManager.AddBlessing(selectedPrize);
    }
}

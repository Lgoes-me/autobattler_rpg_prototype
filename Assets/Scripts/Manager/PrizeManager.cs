using System.Collections.Generic;

public class PrizeManager
{
    private InterfaceManager InterfaceManager { get; set; }
    private PauseManager PauseManager { get; set; }
    
    public void Prepare()
    {
        InterfaceManager = Application.Instance.InterfaceManager;
        PauseManager = Application.Instance.PauseManager;
    }

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
    
    public async void CreatePartyMemberPrize(List<BasePawn> pawns)
    {
        PauseManager.PauseGame();
        var prizes = new PartyMemberPrize(pawns);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        PauseManager.ResumeGame();
        //Application.Instance.PartyManager.(selectedPrize);
    }
}

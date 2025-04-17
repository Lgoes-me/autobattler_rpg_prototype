using System.Collections.Generic;

public class PrizeManager : IManager
{
    private GameSaveManager GameSaveManager { get; set; }
    private ContentManager ContentManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private InterfaceManager InterfaceManager { get; set; }
    private PauseManager PauseManager { get; set; }
    
    public void Prepare()
    {
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        ContentManager = Application.Instance.GetManager<ContentManager>();
        BlessingManager = Application.Instance.GetManager<BlessingManager>();
        PartyManager = Application.Instance.GetManager<PartyManager>();
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
        PauseManager = Application.Instance.GetManager<PauseManager>();
    }

    public async void CreateLevelUpPrize()
    {
        PauseManager.PauseGame();
        
        var prizes = new LevelUpPrize(3, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        PauseManager.ResumeGame();
        selectedPrize.LevelUp();
        GameSaveManager.SavePawnInfo(selectedPrize);
        PartyManager.UpdatePawn(selectedPrize);
    }

    public async void CreateBlessingPrize(List<BlessingIdentifier> blessings)
    {
        PauseManager.PauseGame();
        var prizes = new BlessingPrize(3, blessings);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        PauseManager.ResumeGame();
        BlessingManager.AddBlessing(selectedPrize);
    }
    
    public async void CreatePartyMemberPrize(List<BasePawn> pawns)
    {
        PauseManager.PauseGame();
        var prizes = new PartyMemberPrize(pawns);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        PauseManager.ResumeGame();
        //Application.Instance.PartyManager.(selectedPrize);
    }

    public async void CreateWeaponPrize()
    {
        PauseManager.PauseGame();
        var prizes = new WeaponPrize(3, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.SetWeapon();
        
        PauseManager.ResumeGame();
        GameSaveManager.SavePawnInfo(selectedPrize.PawnInfo);
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);
    }
}

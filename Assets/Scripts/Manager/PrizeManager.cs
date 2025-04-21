using System.Collections.Generic;

public class PrizeManager : IManager
{
    private GameSaveManager GameSaveManager { get; set; }
    private ContentManager ContentManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private ConsumableManager ConsumableManager { get; set; }
    private InterfaceManager InterfaceManager { get; set; }
    private PauseManager PauseManager { get; set; }
    
    public void Prepare()
    {
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        ContentManager = Application.Instance.GetManager<ContentManager>();
        BlessingManager = Application.Instance.GetManager<BlessingManager>();
        PartyManager = Application.Instance.GetManager<PartyManager>();
        ConsumableManager = Application.Instance.GetManager<ConsumableManager>();
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
    
    public async void CreatePartyMemberPrize()
    {
        PauseManager.PauseGame();
        var prizes = new PartyMemberPrize(3, 1, PartyManager.Party, ContentManager);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        PauseManager.ResumeGame();
        var playerControllerPosition = Application.Instance.GetManager<PlayerManager>().PlayerController.transform.position;
        
        GameSaveManager.AddToSelectedParty(selectedPrize);
        PartyManager.AddToCurrentParty(playerControllerPosition, selectedPrize);
        PartyManager.SetPartyToFollow(false);
    }

    public async void CreateWeaponPrize()
    {
        PauseManager.PauseGame();
        var prizes = new WeaponPrize(3, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.ApplyPrize();
        
        PauseManager.ResumeGame();
        GameSaveManager.SavePawnInfo(selectedPrize.PawnInfo);
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);
    }
    
    public async void CreateAbilityPrize()
    {
        PauseManager.PauseGame();
        var prizes = new AbilityPrize(3, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.ApplyPrize();
        
        PauseManager.ResumeGame();
        GameSaveManager.SavePawnInfo(selectedPrize.PawnInfo);
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);
    }
    
    public async void CreateBuffPrize()
    {
        PauseManager.PauseGame();
        var prizes = new BuffPrize(3, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.ApplyPrize();
        
        PauseManager.ResumeGame();
        GameSaveManager.SavePawnInfo(selectedPrize.PawnInfo);
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);
    }
    
    public async void CreateConsumablePrize()
    {
        PauseManager.PauseGame();
        var prizes = new ConsumablePrize(3, ContentManager);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        PauseManager.ResumeGame();
        ConsumableManager.AddConsumable(selectedPrize);
    }
}

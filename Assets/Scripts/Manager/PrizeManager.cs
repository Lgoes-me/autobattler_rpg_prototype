using System.Collections.Generic;

public class PrizeManager : IManager
{
    private GameSaveManager GameSaveManager { get; set; }
    private ContentManager ContentManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private InterfaceManager InterfaceManager { get; set; }
    private PauseManager PauseManager { get; set; }
    private PlayerManager PlayerManager { get; set; }

    private int NumberOfOptions => 3 + PlayerManager.PlayerStats.GetStat(Stat.OpcoesExtras);
    
    public void Prepare()
    {
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        ContentManager = Application.Instance.GetManager<ContentManager>();
        BlessingManager = Application.Instance.GetManager<BlessingManager>();
        PartyManager = Application.Instance.GetManager<PartyManager>();
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
        PauseManager = Application.Instance.GetManager<PauseManager>();
        PlayerManager = Application.Instance.GetManager<PlayerManager>();
    }

    private void PreparePrize()
    {
        PauseManager.PauseGame();
        PlayerManager.DisablePlayerInput();
    }
    
    private void CompletePrize()
    {
        GameSaveManager.SaveCurrentGameState();
        PauseManager.ResumeGame();
        PlayerManager.EnablePlayerInput();
    }
    
    public async void CreateLevelUpPrize()
    {
        PreparePrize();
        
        var prizes = new LevelUpPrize(NumberOfOptions, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        var pawnInfo = selectedPrize.PawnInfo;
        
        pawnInfo.LevelUp();
        PartyManager.UpdatePawn(pawnInfo);

        CompletePrize();
    }

    public async void CreateBlessingPrize(List<BlessingData> blessings = null)
    {
        PreparePrize();

        var prizes = new BlessingPrize(NumberOfOptions, ContentManager, blessings);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        BlessingManager.AddBlessing(selectedPrize.Blessing);
        
        CompletePrize();
    }
    
    public async void CreatePartyMemberPrize()
    {
        PreparePrize();

        var prizes = new PartyMemberPrize(NumberOfOptions, 1, PartyManager.Party, ContentManager);
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);
        
        var pawnInfo = selectedPrize.PawnInfo;
        
        GameSaveManager.AddToSelectedParty(pawnInfo);
        
        PartyManager.AddToCurrentParty(Application.Instance.GetManager<PlayerManager>().PlayerTransform.position, pawnInfo, true);
        PartyManager.SetPartyToFollow(false);
        
        CompletePrize();
    }

    public async void CreateWeaponPrize()
    {
        PreparePrize();

        var prizes = new WeaponPrize(NumberOfOptions, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.ApplyPrize();
        
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);
        
        CompletePrize();
    }
    
    public async void CreateAbilityPrize()
    {
        PreparePrize();

        var prizes = new AbilityPrize(NumberOfOptions, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.ApplyPrize();
        
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);
        
        CompletePrize();
    }
    
    public async void CreateBuffPrize()
    {
        PreparePrize();
        
        var prizes = new BuffPrize(NumberOfOptions, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.ApplyPrize();
        
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);
        
        CompletePrize();
    }
    
    public async void CreateConsumablePrize()
    {
        PreparePrize();

        var prizes = new ConsumablePrize(NumberOfOptions, ContentManager, GameSaveManager.GetSelectedParty());
        var selectedPrize = await InterfaceManager.ShowPrizeCanvas(prizes);

        selectedPrize.ApplyPrize();
        
        PartyManager.UpdatePawn(selectedPrize.PawnInfo);

        CompletePrize();
    }
}

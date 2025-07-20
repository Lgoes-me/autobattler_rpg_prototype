using System.Collections.Generic;

public class BlessingManager : IManager
{
    public List<BlessingData> Blessings { get; private set; }

    private ContentManager ContentManager { get; set; }
    private InterfaceManager InterfaceManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private BattleEventsManager BattleEventsManager { get; set; }

    public BlessingManager()
    {
        Blessings = new List<BlessingData>();
    }

    public void Prepare()
    {
        ContentManager = Application.Instance.GetManager<ContentManager>();
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        BattleEventsManager = Application.Instance.GetManager<BattleEventsManager>();
    }

    public void LoadBlessings()
    {
        var blessings = GameSaveManager.GetBlessings();

        foreach (var blessing in blessings)
        {
            AddBlessing(blessing, false);
        }
    }

    public void AddBlessing(BlessingIdentifier identifier, bool newBlessing = true)
    {
        var blessing = ContentManager.GetBlessingFromIdAndRarity(identifier, Rarity.Common);
        blessing.DoBlessingCreatedEvent();

        Blessings.Add(blessing);

        if (newBlessing)
        {
            BattleEventsManager.DoBlessingGainedEvent();
        }

        InterfaceManager.UpdateBlessingsCanvas(Blessings);

        GameSaveManager.SaveCurrentGameState();
    }

    public void ClearBlessings()
    {
        Blessings.Clear();
        InterfaceManager.UpdateBlessingsCanvas(Blessings);

        GameSaveManager.SaveCurrentGameState();
    }
}
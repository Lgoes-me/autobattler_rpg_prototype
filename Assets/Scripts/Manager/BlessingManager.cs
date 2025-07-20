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
        var blessingIdentifiers = GameSaveManager.GetBlessings();

        foreach (var identifier in blessingIdentifiers)
        {
            var blessing = ContentManager.GetBlessingFromIdAndRarity(identifier, Rarity.Common);
            AddBlessing(blessing, false);
        }
    }

    public void AddBlessing(BlessingData blessing, bool newBlessing = true)
    {
        blessing.DoBlessingCreatedEvent();

        Blessings.Add(blessing);

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
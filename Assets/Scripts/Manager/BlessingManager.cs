using System.Collections.Generic;

public class BlessingManager : IManager
{
    public List<Blessing> Blessings { get; private set; }
    private BlessingFactory BlessingFactory { get; set; }

    private InterfaceManager InterfaceManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private BattleEventsManager BattleEventsManager { get; set; }

    public BlessingManager()
    {
        BlessingFactory = new BlessingFactory();
        Blessings = new List<Blessing>();
    }

    public void Prepare()
    {
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
        var blessing = BlessingFactory.CreateBlessing(identifier);
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
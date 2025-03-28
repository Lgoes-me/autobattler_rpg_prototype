using System.Collections.Generic;
using System.Linq;

public class BlessingManager : IManager
{
    public List<Blessing> Blessings { get; private set; }
    private BlessingFactory BlessingFactory { get; set; }

    private InterfaceManager InterfaceManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }

    public BlessingManager()
    {
        BlessingFactory = new BlessingFactory();
        Blessings = new List<Blessing>();
    }
    
    public void Prepare()
    {
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
    }

    public void LoadBlessings()
    {
        Blessings = GameSaveManager.GetBlessings().Select(j => BlessingFactory.CreateBlessing(j)).ToList();
        InterfaceManager.InitBlessingsCanvas(Blessings);
    }

    public void AddBlessing(BlessingIdentifier identifier)
    {
        Blessings.Add(BlessingFactory.CreateBlessing(identifier));
        GameSaveManager.SetBlessings();
        InterfaceManager.InitBlessingsCanvas(Blessings);
    }

    public void RemoveBlessing(BlessingIdentifier identifier)
    {
        var jokerToRemove = Blessings.FirstOrDefault(j => j.Identifier == identifier);
        if (jokerToRemove == null)
            return;

        Blessings.Remove(jokerToRemove);
        GameSaveManager.SetBlessings();
        InterfaceManager.InitBlessingsCanvas(Blessings);
    }

    public void ClearBlessings()
    {
        Blessings.Clear();
        GameSaveManager.SetBlessings();
        InterfaceManager.InitBlessingsCanvas(Blessings);
    }

    public void ReorderBlessings(List<BlessingIdentifier> identifiers)
    {
        Blessings = identifiers.Select(j => BlessingFactory.CreateBlessing(j)).ToList();
        GameSaveManager.SetBlessings();
        InterfaceManager.InitBlessingsCanvas(Blessings);
    }
}
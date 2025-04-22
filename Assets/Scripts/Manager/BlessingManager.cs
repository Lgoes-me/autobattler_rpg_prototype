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
        InterfaceManager.UpdateBlessingsCanvas(Blessings);
    }

    public void AddBlessing(BlessingIdentifier identifier)
    {
        Blessings.Add(BlessingFactory.CreateBlessing(identifier));
        GameSaveManager.SetBlessings();
        InterfaceManager.UpdateBlessingsCanvas(Blessings);
    }
    
    public void ClearBlessings()
    {
        Blessings.Clear();
        GameSaveManager.SetBlessings();
        InterfaceManager.UpdateBlessingsCanvas(Blessings);
    }
}
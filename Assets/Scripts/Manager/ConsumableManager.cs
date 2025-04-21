using System.Collections.Generic;
using System.Linq;

public class ConsumableManager : IManager
{
    private List<ConsumableData> Consumables { get; set; }

    private GameSaveManager GameSaveManager { get; set; }
    private ContentManager ContentManager { get; set; }
    private InterfaceManager InterfaceManager { get; set; }

    public void Prepare()
    {
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        ContentManager = Application.Instance.GetManager<ContentManager>();
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
    }

    public ConsumableManager()
    {
        Consumables = new List<ConsumableData>();
    }

    public void LoadConsumables()
    {
        Consumables = GameSaveManager.GetConsumables().Select(j => ContentManager.GetConsumableFromId(j)).ToList();
        InterfaceManager.InitConsumablesCanvas(Consumables);
    }

    public void AddConsumable(ConsumableData consumable)
    {
        Consumables.Add(consumable);
        GameSaveManager.SetConsumables(Consumables);
        InterfaceManager.InitConsumablesCanvas(Consumables);
    }

    public void RemoveConsumable(ConsumableData consumable)
    {
        Consumables.Remove(consumable);
        GameSaveManager.SetConsumables(Consumables);
        InterfaceManager.InitConsumablesCanvas(Consumables);
    }
    
    public void ClearConsumables()
    {
        Consumables.Clear();
        GameSaveManager.SetConsumables(Consumables);
        InterfaceManager.InitConsumablesCanvas(Consumables);
    }
}
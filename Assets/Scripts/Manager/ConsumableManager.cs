using System.Collections.Generic;

public class ConsumableManager : IManager
{
    private List<ConsumableData> Consumables { get; set; }

    public ConsumableManager()
    {
        Consumables = new List<ConsumableData>();
    }

    public void AddConsumable(ConsumableData consumable)
    {
        Consumables.Add(consumable);
        Application.Instance.GetManager<GameSaveManager>().SetConsumables(Consumables);
        Application.Instance.GetManager<InterfaceManager>().InitConsumablesCanvas(Consumables);
    }
}
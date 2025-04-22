using System.Collections.Generic;

public class ConsumableCanvasHolderController : BaseCanvasHolderController<ConsumableCanvasController, ConsumableData>
{
    public void StartBattle()
    {
        Items ??= new List<ConsumableCanvasController>();
        
        foreach (var consumableCanvas in Items)
        {
            consumableCanvas.StartBattle();
        }
    }

    public void FinishBattle()
    {
        Items ??= new List<ConsumableCanvasController>();
        
        foreach (var consumableCanvas in Items)
        {
            consumableCanvas.FinishBattle();
        }
    }
}
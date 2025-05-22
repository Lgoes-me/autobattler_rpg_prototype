using System.Collections.Generic;

public class ConsumableCanvasHolderController : BaseCanvasHolderController<ConsumableCanvasController, ConsumableCanvasControllerData>
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

public class ConsumableCanvasControllerData
{
    public Pawn Pawn { get; }
    public ConsumableData ConsumableData { get; }

    public ConsumableCanvasControllerData(Pawn pawn, ConsumableData consumableData)
    {
        Pawn = pawn;
        ConsumableData = consumableData;
    }
}
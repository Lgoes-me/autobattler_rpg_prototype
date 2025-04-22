using System.Collections.Generic;
using UnityEngine;

public class ConsumableCanvasHolderController : MonoBehaviour
{
    [field: SerializeField] private ConsumableCanvasController ConsumableCanvasPrefab { get; set; }
    [field: SerializeField] private RectTransform ConsumableCanvasParent { get; set; }
    
    private List<ConsumableCanvasController> ConsumableCanvases { get; set; }

    private void Awake()
    {
        ConsumableCanvases = new List<ConsumableCanvasController>();
    }
    
    public void UpdateConsumablesCanvas(List<ConsumableData> consumables)
    {
        foreach (var consumableCanvas in ConsumableCanvases)
        {
            Destroy(consumableCanvas.gameObject);
        }

        ConsumableCanvases.Clear();

        foreach (var consumable in consumables)
        {
            var consumableCanvasController = Instantiate(ConsumableCanvasPrefab, ConsumableCanvasParent).Init(consumable);
            ConsumableCanvases.Add(consumableCanvasController);
        }
    }
    
    public void StartBattle()
    {
        foreach (var consumableCanvas in ConsumableCanvases)
        {
            consumableCanvas.StartBattle();
        }
    }

    public void FinishBattle()
    {
        foreach (var consumableCanvas in ConsumableCanvases)
        {
            consumableCanvas.FinishBattle();
        }
    }
}
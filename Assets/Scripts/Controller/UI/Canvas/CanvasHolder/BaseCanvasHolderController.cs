using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCanvasHolderController<T,T2> : BaseCanvasController where T : BaseCanvasHolderItemController<T2>
{
    [field: SerializeField] private T Prefab { get; set; }
    [field: SerializeField] private RectTransform Parent { get; set; }
    
    protected List<T> Items { get; set; }

    public void UpdateItems(List<T2> items)
    {
        Items ??= new List<T>();

        foreach (var item in Items)
        {
            Destroy(item.gameObject);
        }

        Items.Clear();

        foreach (var item in items)
        {
            var canvasHolderItemController = Instantiate(Prefab, Parent).Init(item);
            Items.Add(canvasHolderItemController as T);
        }
    }
}
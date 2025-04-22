using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCanvasHolderController<T,T2> : BaseCanvasController where T : BaseCanvasHolderItemController<T2>
{
    [field: SerializeField] protected T Prefab { get; private set; }
    [field: SerializeField] protected RectTransform Parent { get;  private set; }
    
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
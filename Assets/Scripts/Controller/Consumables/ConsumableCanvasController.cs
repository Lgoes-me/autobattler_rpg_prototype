using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableCanvasController : BaseCanvasController, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField]
    private ConsumableData ConsumableData { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    
    private bool IsDragging { get; set; }
    private Vector3 StartingPosition { get; set; }
    
    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        Name.SetText(ConsumableData.Id);
        Show();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        StartingPosition = transform.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        IsDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        transform.position = StartingPosition;
    }

    private void Update()
    {
        if(!IsDragging)
            return;

        transform.position = Input.mousePosition;
    }
}

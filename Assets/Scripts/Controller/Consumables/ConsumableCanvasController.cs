using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableCanvasController : BaseCanvasController, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField] private LayerMask Mask { get; set; }
    
    private ConsumableData Consumable { get; set; }
    
    private bool IsDragging { get; set; }
    private Vector3 StartingPosition { get; set; }

    public ConsumableCanvasController Init(ConsumableData consumable)
    {
        Consumable = consumable;
        
        Name.SetText(Consumable.Id);

        Show();
        
        return this;
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
        CanvasGroup.blocksRaycasts = false;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast (ray, out var hit, 10000, Mask))
        {
            var pawn = hit.transform.GetComponent<PawnController>();
            var effect = Consumable.Effect.ToDomain(pawn);
            
            effect.DoAbilityEffect(pawn);
            
            Destroy(this.gameObject);
            return;
        }
        
        CanvasGroup.blocksRaycasts = true;
        
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

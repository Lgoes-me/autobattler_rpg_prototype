using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableCanvasController : BaseCanvasHolderItemController<ConsumableData>, 
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField] private LayerMask Mask { get; set; }

    private ConsumableData Consumable { get; set; }

    private bool CanBeUsed => Consumable.UsableOutsideCombat || IsInBattle;
    private bool IsInBattle { get; set; }
    private bool IsDragging { get; set; }
    private Vector3 StartingPosition { get; set; }
    
    public override BaseCanvasHolderItemController<ConsumableData> Init(ConsumableData consumable)
    {
        Consumable = consumable;

        Name.SetText(Consumable.Id);

        Show();

        if (!Consumable.UsableOutsideCombat)
        {
            CanvasGroup.alpha = 0.5f;
            CanvasGroup.blocksRaycasts = false;
        }
        
        return this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartingPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanBeUsed)
            return;

        IsDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanBeUsed)
            return;

        CanvasGroup.blocksRaycasts = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 10000, Mask))
        {
            var pawn = hit.transform.GetComponent<PawnController>();
            var effect = Consumable.Effect.ToDomain(pawn);

            effect.DoAbilityEffect(pawn);
            Application.Instance.GetManager<ConsumableManager>().RemoveConsumable(Consumable);
            return;
        }

        CanvasGroup.blocksRaycasts = true;

        IsDragging = false;
        transform.position = StartingPosition;
    }

    private void Update()
    {
        if (!IsDragging || !CanBeUsed)
            return;

        transform.position = Input.mousePosition;
    }

    public void StartBattle()
    {
        IsInBattle = true;

        if (Consumable.UsableOutsideCombat)
            return;
        
        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;
    }

    public void FinishBattle()
    {
        IsInBattle = false;

        if (Consumable.UsableOutsideCombat) 
            return;
        
        CanvasGroup.alpha = 0.5f;
        CanvasGroup.blocksRaycasts = false;
    }
}
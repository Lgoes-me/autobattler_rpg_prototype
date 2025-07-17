using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableCanvasController : BaseCanvasHolderItemController<ConsumableCanvasControllerData>, 
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField] private LayerMask Mask { get; set; }


    private bool CanBeUsed => Data.Consumable.UsableOutsideCombat || IsInBattle;
    private bool IsInBattle { get; set; }
    private bool IsDragging { get; set; }
    private Vector3 StartingPosition { get; set; }
    
    public override BaseCanvasHolderItemController<ConsumableCanvasControllerData> Init(ConsumableCanvasControllerData data)
    {
        Data = data;
        Name.SetText(Data.Consumable.Id);

        Show();

        if (!Data.Consumable.UsableOutsideCombat)
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

        var mouseInput = Application.Instance.GetManager<InputManager>().MousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out var hit, 10000, Mask))
        {
            var pawn = hit.transform.GetComponent<PawnController>();

            if (pawn.Pawn.Team != Data.Consumable.Focus)
            {
                return;
            }
            
            var effect = Data.Consumable.Effect.ToDomain(pawn);

            effect.DoAbilityEffect(pawn);
            Data.Pawn.GetComponent<ConsumableComponent>().RemoveConsumable(Data.Consumable);
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

        transform.position = Application.Instance.GetManager<InputManager>().MousePosition;
    }

    public void StartBattle()
    {
        IsInBattle = true;

        if (Data.Consumable.UsableOutsideCombat)
            return;
        
        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;
    }

    public void FinishBattle()
    {
        IsInBattle = false;

        if (Data.Consumable.UsableOutsideCombat) 
            return;
        
        CanvasGroup.alpha = 0.5f;
        CanvasGroup.blocksRaycasts = false;
    }
}
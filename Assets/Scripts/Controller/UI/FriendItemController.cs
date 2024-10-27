using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FriendItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private TextMeshProUGUI PawnName { get; set; }

    public bool IsInParty { get; set; }
    public PawnData PawnData { get; private set; }
    private Action<FriendItemController> OnDragEnd { get; set; }

    private bool IsDragging { get; set; }
    private Vector3 StartingPosition { get; set; }

    public FriendItemController Init(bool isInParty, PawnData pawnData, Action<FriendItemController> onDragEnd)
    {
        IsInParty = isInParty;
        PawnData = pawnData;
        OnDragEnd = onDragEnd;
        IsDragging = false;

        PawnName.SetText(PawnData.Id);
        return this;
    }

    private void Update()
    {
        if (!IsDragging)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartingPosition = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        IsDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        OnDragEnd(this);
    }

    public void ResetPosition()
    {
        transform.localPosition = StartingPosition;
    }
}
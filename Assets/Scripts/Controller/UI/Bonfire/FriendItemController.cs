using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private TextMeshProUGUI PawnName { get; set; }
    [field: SerializeField] private Image Background { get; set; }

    public PawnData PawnData { get; private set; }
    public FriendItemState State { get; private set; }
    public BonfireScene BonfireScene { get; private set; }

    private bool IsDragging { get; set; }
    private Vector3 StartingPosition { get; set; }

    public FriendItemController Init(
        PawnData pawnData,
        BonfireScene bonfireScene,
        FriendItemState state)
    {
        PawnData = pawnData;
        BonfireScene = bonfireScene;
        State = state;

        switch (state)
        {
            case FriendItemState.Active:
                Background.color = Color.green;
                break;
            case FriendItemState.Inactive:
                Background.color = Color.gray;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

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
        if (State == FriendItemState.Inactive)
            return;

        StartingPosition = transform.localPosition;
        BonfireScene.BonfirePanel?.OnPick(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (State == FriendItemState.Inactive)
            return;

        IsDragging = true;
        BonfireScene.BonfirePanel?.OnHover(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (State == FriendItemState.Inactive)
            return;

        IsDragging = false;
        BonfireScene.BonfirePanel?.OnDrop(this);
    }

    public void Activate()
    {
        State = FriendItemState.Active;
    }

    public void ResetPosition()
    {
        transform.localPosition = StartingPosition;
    }
}

public enum FriendItemState
{
    Active,
    Inactive
}
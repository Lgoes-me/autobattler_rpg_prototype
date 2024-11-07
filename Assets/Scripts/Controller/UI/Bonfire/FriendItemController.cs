using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendItemController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private TextMeshProUGUI PawnName { get; set; }
    [field: SerializeField] private Image Background { get; set; }
    [field: SerializeField] private GraphicRaycaster GraphicRaycaster { get; set; }

    public PawnFacade Pawn { get; private set; }
    private FriendItemState State { get; set; }
    private BonfireScene BonfireScene { get; set; }
    private IBonfirePanel BonfirePanel { get; set; }

    private bool IsDragging { get; set; }

    public FriendItemController Init(
        PawnFacade pawn,
        BonfireScene bonfireScene,
        IBonfirePanel bonfirePanel,
        FriendItemState state)
    {
        Pawn = pawn;
        BonfireScene = bonfireScene;
        BonfirePanel = bonfirePanel;
        State = state;

        UpdateState();

        IsDragging = false;

        PawnName.SetText(Pawn.Id);
        return this;
    }

    private void UpdateState()
    {
        switch (State)
        {
            case FriendItemState.Active:
                Background.color = Color.green;
                break;
            case FriendItemState.Inactive:
                Background.color = Color.gray;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
    }

    private void Update()
    {
        if (!IsDragging)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (State == FriendItemState.Inactive)
            return;
        
        if (BonfireScene.BonfirePanel != null)
        {
            BonfireScene.BonfirePanel.OnClick(this);
        }
        else
        {
            BonfirePanel.OnClick(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (State == FriendItemState.Inactive)
            return;

        if (BonfireScene.BonfirePanel != null)
        {
            BonfireScene.BonfirePanel.OnPick(this);
        }
        else
        {
            BonfirePanel.OnPick(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (State == FriendItemState.Inactive)
            return;

        IsDragging = true;
        BonfireScene.IsDragging = true;
        GraphicRaycaster.enabled = false;
        
        if (BonfireScene.BonfirePanel != null && BonfireScene.BonfirePanel.CanDrop())
        {
            BonfireScene.BonfirePanel.OnHover(this);
        }
        else
        {
            BonfirePanel.OnHover(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (State == FriendItemState.Inactive)
            return;

        IsDragging = false;
        BonfireScene.IsDragging = false;
        GraphicRaycaster.enabled = true;
        
        if (BonfireScene.BonfirePanel != null && BonfireScene.BonfirePanel.CanDrop())
        {
            BonfireScene.BonfirePanel.OnDrop(this);
        }
        else
        {
            BonfirePanel.OnDrop(this);
        }
    }

    public void ChangeState(FriendItemState state)
    {
        State = state;
        UpdateState();
    }
}

public enum FriendItemState
{
    Active,
    Inactive
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyPanelController : BaseFriendItemPanelController
{
    [field: SerializeField] private Transform PartyDivider { get; set; }

    public List<PawnData> Party =>
        Content.GetComponentsInChildren<FriendItemController>().Select(f => f.PawnData).ToList();

    public override void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        base.Init(partyManager, bonfireScene);

        foreach (var pawnController in PartyManager.Party)
        {
            var pawnData = PartyManager.AvailableParty.First(p => pawnController.Pawn.Id == p.Id);
            Instantiate(FriendItemPrefab, Content).Init(pawnData, bonfireScene, this, FriendItemState.Active);
        }

        PartyDivider.gameObject.SetActive(false);
    }

    public override void OnPick(FriendItemController friendItemController)
    {
        base.OnPick(friendItemController);

        PartyDivider.gameObject.SetActive(true);
        friendItemController.transform.SetParent(transform.parent);

        if (transform.childCount == 2)
        {
            Content.GetComponentsInChildren<FriendItemController>().FirstOrDefault()?.ChangeState(FriendItemState.Inactive);
        }
    }

    public override void OnHover(FriendItemController friendItemController)
    {
        base.OnHover(friendItemController);
        
        var items = Content.GetComponentsInChildren<FriendItemController>();
        var under = items.FirstOrDefault(i => i.transform.position.y <= Input.mousePosition.y);
        
        PartyDivider.SetSiblingIndex(under != null
            ? Mathf.Clamp(under.transform.GetSiblingIndex() - 1, 0, Content.childCount)
            : Content.childCount);
    }

    public override bool CanDrop()
    {
        var items = Content.GetComponentsInChildren<FriendItemController>();
        return items.Length < PartyManager.PartySizeLimit;
    }

    public override void OnDrop(FriendItemController friendItemController)
    {
        PartyDivider.gameObject.SetActive(false);
        friendItemController.transform.SetParent(Content);
        
        var items = Content.GetComponentsInChildren<FriendItemController>();
        var under = items.FirstOrDefault(i => i.transform.position.y <= Input.mousePosition.y);

        if (under != null)
        {
            friendItemController.transform.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
        }

        BonfireScene.SaveChanges();

        if (transform.childCount > 2)
        {
            Content.GetComponentsInChildren<FriendItemController>().FirstOrDefault()?.ChangeState(FriendItemState.Active);
        }

        base.OnDrop(friendItemController);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        PartyDivider.gameObject.SetActive(BonfireScene.IsDragging);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        PartyDivider.gameObject.SetActive(false);
    }
}
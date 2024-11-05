using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyPanelController : BaseFriendItemPanelController
{
    [field: SerializeField] private Transform PartyDivider { get; set; }

    public List<PawnData> Party => Content.GetComponentsInChildren<FriendItemController>().Select(f => f.PawnData).ToList();

    public override void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        base.Init(partyManager, bonfireScene);
        
        foreach (var pawnController in PartyManager.Party)
        {
            var pawnData = PartyManager.AvailableParty.First(p => pawnController.Pawn.Id == p.Id);
            FriendItems.Add(Instantiate(FriendItemPrefab, Content).Init(pawnData, bonfireScene, this, FriendItemState.Active));
        }
        
        PartyDivider.gameObject.SetActive(false);
    }

    public override void OnPick(FriendItemController friendItemController)
    {
        base.OnPick(friendItemController);
        
        PartyDivider.gameObject.SetActive(true);
        friendItemController.transform.SetParent(transform.parent);

        if (FriendItems.Count == 1)
        {
            FriendItems[0].ChangeState(FriendItemState.Inactive);
        }
    }

    public override void OnHover(FriendItemController friendItemController)
    {
        base.OnHover(friendItemController);

        var under = FriendItems.FirstOrDefault(i => i.transform.position.y <= Input.mousePosition.y);

        if (under != null)
        {
            PartyDivider.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
        }
        else
        {
            PartyDivider.SetSiblingIndex(Content.childCount);
        }
    }

    public override void OnDrop(FriendItemController friendItemController)
    {
        base.OnDrop(friendItemController);
        PartyDivider.gameObject.SetActive(false);
        
        friendItemController.transform.SetParent(Content);

        var under = FriendItems.FirstOrDefault(i => i.transform.position.y <= Input.mousePosition.y);

        if (under != null)
        {
            friendItemController.transform.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
        }

        FriendItems.Add(friendItemController);
        BonfireScene.SaveChanges();
            
        if (FriendItems.Count > 1)
        {
            FriendItems[0].ChangeState(FriendItemState.Active);
        }
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

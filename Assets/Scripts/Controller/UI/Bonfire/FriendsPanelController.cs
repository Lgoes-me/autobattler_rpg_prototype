using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FriendsPanelController : MonoBehaviour, IBonfirePanel, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] private RectTransform FriendsContent { get; set; }
    [field: SerializeField] private FriendItemController FriendItemPrefab { get; set; }
    
    private PartyManager PartyManager { get; set; }
    private BonfireScene BonfireScene { get; set; }
    private List<FriendItemController> FriendItems { get; set; }

    public void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        FriendItems = new List<FriendItemController>();
        PartyManager = partyManager;
        BonfireScene = bonfireScene;
        
        var friends = partyManager.AvailableParty;
        var party = partyManager.Party;
        
        foreach (var pawnData in friends)
        {
            var state = party.Any(i => i.Pawn.Id == pawnData.Id) ? FriendItemState.Inactive : FriendItemState.Active;
            var friendItem = Instantiate(FriendItemPrefab, FriendsContent).Init(pawnData, bonfireScene, state);
            
            FriendItems.Add(friendItem);
        }
    }

    public void OnPick(FriendItemController friendItemController)
    {
        var pawnData = friendItemController.PawnData;
        var index = FriendItems.IndexOf(friendItemController);
        var friendItem = Instantiate(FriendItemPrefab, FriendsContent).Init(pawnData, BonfireScene, FriendItemState.Inactive);
        FriendItems.Add(friendItem);
        friendItem.transform.SetSiblingIndex(index);
        friendItemController.transform.SetParent(transform.parent);
    }

    public void OnHover(FriendItemController friendItemController)
    {
        
    }

    public void OnDrop(FriendItemController friendItemController)
    {
        var pawnData = friendItemController.PawnData;
        var inactiveItem = FriendItems.FirstOrDefault(i => i.PawnData == pawnData);
        
        Destroy(friendItemController.gameObject);
        
        if(inactiveItem == null)
            return;
        
        inactiveItem.Activate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BonfireScene.BonfirePanel = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BonfireScene.BonfirePanel = null;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FriendsPanelController : MonoBehaviour, IBonfirePanel
{
    [field: SerializeField] private FriendItemController FriendItemPrefab { get; set; }
    [field: SerializeField] private RectTransform Content { get; set; }

    private PartyManager PartyManager { get; set; }
    private BonfireScene BonfireScene { get; set; }
    private List<FriendItemController> PartyItems { get; set; }
    
    public void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        PartyManager = partyManager;
        BonfireScene = bonfireScene;
        PartyItems = new List<FriendItemController>();

        var friends = PartyManager.AvailableParty;
        var party = PartyManager.Party;

        foreach (var pawnData in friends)
        {
            var state = party.Any(i => i.Pawn.Id == pawnData.Id) ? FriendItemState.Inactive : FriendItemState.Active;
            var item = Instantiate(FriendItemPrefab, Content).Init(pawnData, bonfireScene, this, state);
            PartyItems.Add(item);
        }
    }
    
    public void OnClick(FriendItemController friendItemController)
    {
        BonfireScene.Select(friendItemController.PawnData);
    }
    
    public void OnPick(FriendItemController friendItemController)
    {
        var pawnData = friendItemController.PawnData;
        var index = friendItemController.transform.GetSiblingIndex();
        
        BonfireScene.Select(friendItemController.PawnData);
        
        var friendItem = Instantiate(FriendItemPrefab, Content).Init(pawnData, BonfireScene, this, FriendItemState.Inactive);
        
        friendItem.transform.SetSiblingIndex(index);
        friendItemController.transform.SetParent(transform.parent);
    }

    public void OnHover(FriendItemController friendItemController)
    {
        
    }

    public bool CanDrop()
    {
        return true;
    }
    
    public void OnDrop(FriendItemController friendItemController)
    {
        var pawnData = friendItemController.PawnData;
        Destroy(friendItemController.gameObject);
        
        var inactiveItem = PartyItems.First(i => i.PawnData.Id == pawnData.Id);
        inactiveItem.ChangeState(FriendItemState.Active);
        
        BonfireScene.SaveChanges();
        
        BonfireScene.Unselect();
        BonfireScene.UpdateArchetypes();
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
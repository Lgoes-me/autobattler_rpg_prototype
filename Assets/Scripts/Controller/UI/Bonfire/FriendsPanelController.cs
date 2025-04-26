using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FriendsPanelController : MonoBehaviour, IBonfirePanel
{
    [field: SerializeField] private FriendItemController FriendItemPrefab { get; set; }
    [field: SerializeField] private RectTransform Content { get; set; }
    
    private BonfireScene BonfireScene { get; set; }
    private List<FriendItemController> PartyItems { get; set; }
    
    public void Init(GameSaveManager gameSaveManager, BonfireScene bonfireScene)
    {
        BonfireScene = bonfireScene;
        PartyItems = new List<FriendItemController>();

        var availableParty = gameSaveManager.GetAvailableParty();
        var selectedParty = gameSaveManager.GetSelectedParty();

        foreach (var pawnData in availableParty)
        {
            var state = selectedParty.Any(i => i.Name == pawnData.Id) ? FriendItemState.Inactive : FriendItemState.Active;
            var item = Instantiate(FriendItemPrefab, Content).Init(pawnData, bonfireScene, this, state);
            PartyItems.Add(item);
        }
    }
    
    public void OnClick(FriendItemController friendItemController)
    {
        BonfireScene.Select(friendItemController.Pawn);
    }
    
    public void OnPick(FriendItemController friendItemController)
    {
        var pawn = friendItemController.Pawn;
        var index = friendItemController.transform.GetSiblingIndex();
        
        BonfireScene.Select(pawn);
        var friendItem = Instantiate(FriendItemPrefab, Content).Init(pawn, BonfireScene, this, FriendItemState.Inactive);
        
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
        var pawn = friendItemController.Pawn;
        Destroy(friendItemController.gameObject);
        
        var inactiveItem = PartyItems.First(i => i.Pawn.Id == pawn.Id);
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
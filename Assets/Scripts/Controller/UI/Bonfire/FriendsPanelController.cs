using System.Linq;

public class FriendsPanelController : BaseFriendItemPanelController
{
    public override void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        base.Init(partyManager, bonfireScene);

        var friends = PartyManager.AvailableParty;
        var party = PartyManager.Party;

        foreach (var pawnData in friends)
        {
            var state = party.Any(i => i.Pawn.Id == pawnData.Id) ? FriendItemState.Inactive : FriendItemState.Active;
            var friendItem = Instantiate(FriendItemPrefab, Content).Init(pawnData, bonfireScene, this, state);

            FriendItems.Add(friendItem);
        }
    }

    public override void OnPick(FriendItemController friendItemController)
    {
        var pawnData = friendItemController.PawnData;
        var index = FriendItems.IndexOf(friendItemController);
        
        base.OnPick(friendItemController);
        
        var friendItem = Instantiate(FriendItemPrefab, Content).Init(pawnData, BonfireScene, this, FriendItemState.Inactive);
        
        FriendItems.Add(friendItem);
        friendItem.transform.SetSiblingIndex(index);
        friendItemController.transform.SetParent(transform.parent);
    }
    
    public override void OnDrop(FriendItemController friendItemController)
    {
        var pawnData = friendItemController.PawnData;
        Destroy(friendItemController.gameObject);
        
        var inactiveItem = FriendItems.First(i => i.PawnData.Id == pawnData.Id);
        inactiveItem.ChangeState(FriendItemState.Active);
        
        BonfireScene.SaveChanges();
        
        base.OnDrop(friendItemController);
    }
}
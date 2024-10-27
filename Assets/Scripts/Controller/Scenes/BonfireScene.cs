using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BonfireScene : BaseScene
{
    [field: SerializeField] private RectTransform FriendsContent { get; set; }
    [field: SerializeField] private RectTransform PartyContent { get; set; }
    [field: SerializeField] private FriendItemController FriendItemPrefab { get; set; }

    [field: SerializeField] private Button FinishButton { get; set; }

    private List<FriendItemController> PartyItems { get; set; }
    private List<FriendItemController> FriendItems { get; set; }
    private BonfireController BonfireController { get; set; }

    public void Init(BonfireController bonfireController)
    {
        PartyItems = new List<FriendItemController>();
        FriendItems = new List<FriendItemController>();
        BonfireController = bonfireController;

        FinishButton.onClick.AddListener(EndBonfireScene);

        var partyManager = Application.Instance.PartyManager;

        foreach (var pawnController in partyManager.Party)
        {
            var pawnData = partyManager.AvailableParty.First(p => pawnController.Pawn.Id == p.Id);
            PartyItems.Add(Instantiate(FriendItemPrefab, PartyContent).Init(true, pawnData, OnMouseDrop));
        }

        foreach (var pawnData in partyManager.AvailableParty)
        {
            if (PartyItems.Any(i => i.PawnData.Id == pawnData.Id))
                continue;

            FriendItems.Add(Instantiate(FriendItemPrefab, FriendsContent).Init(false, pawnData, OnMouseDrop));
        }
    }

    private void SaveChanges()
    {
        var selectedPawns = PartyItems.Select(i => i.PawnData).ToList();
        Application.Instance.PartyManager.SetSelectedParty(selectedPawns);
    }

    private void EndBonfireScene()
    {
        Application.Instance.SceneManager.EndBonfireScene();
        BonfireController.Preselect();
    }

    private void OnMouseDrop(FriendItemController friendItemController)
    {
        if (friendItemController.IsInParty)
        {
            OnPartyItemDrop(friendItemController);
        }
        else
        {
            OnFriendListItemDrop(friendItemController);
        }
    }

    private void OnPartyItemDrop(FriendItemController friendItemController)
    {
        Vector2 localMousePosition = FriendsContent.InverseTransformPoint(Input.mousePosition);

        if (FriendsContent.rect.Contains(localMousePosition))
        {
            friendItemController.IsInParty = false;
            friendItemController.transform.SetParent(FriendsContent);
            PartyItems.Remove(friendItemController);
            FriendItems.Add(friendItemController);
            SaveChanges();
            return;
        }

        friendItemController.ResetPosition();
    }

    private void OnFriendListItemDrop(FriendItemController friendItemController)
    {
        Vector2 localMousePosition = PartyContent.InverseTransformPoint(Input.mousePosition);
        
        if (FriendsContent.rect.Contains(localMousePosition))
        {
            friendItemController.IsInParty = true;
            friendItemController.transform.SetParent(PartyContent);
            FriendItems.Remove(friendItemController);
            PartyItems.Add(friendItemController);
            SaveChanges();
            return;
        }

        friendItemController.ResetPosition();
    }
}
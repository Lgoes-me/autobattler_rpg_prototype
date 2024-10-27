using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BonfireScene : BaseScene
{
    [field: SerializeField] private RectTransform FriendsContent { get; set; }
    [field: SerializeField] private RectTransform PartyContent { get; set; }
    [field: SerializeField] private FriendItemController FriendItemPrefab { get; set; }

    [field: SerializeField] private Transform FriendsDivider { get; set; }
    [field: SerializeField] private Transform PartyDivider { get; set; }
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
            PartyItems.Add(Instantiate(FriendItemPrefab, PartyContent)
                .Init(pawnData, OnMouseHover, OnMouseDrop));
        }

        foreach (var pawnData in partyManager.AvailableParty)
        {
            if (PartyItems.Any(i => i.PawnData.Id == pawnData.Id))
                continue;

            FriendItems.Add(Instantiate(FriendItemPrefab, FriendsContent)
                .Init(pawnData, OnMouseHover, OnMouseDrop));
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
        if (PartyItems.Contains(friendItemController))
        {
            OnPartyItemDrop(friendItemController);
        }
        else
        {
            OnFriendListItemDrop(friendItemController);
        }
    }

    private void OnMouseHover(FriendItemController friendItemController)
    {
        if (PartyItems.Contains(friendItemController))
        {
            OnPartyItemHover();
        }
        else
        {
            OnFriendListHover();
        }
    }

    private void OnPartyItemHover()
    {
        Vector2 localMousePosition = FriendsContent.InverseTransformPoint(Input.mousePosition);

        if (FriendsContent.rect.Contains(localMousePosition))
        {
            FriendsDivider.gameObject.SetActive(true);
            
            var under = FriendItems.FirstOrDefault(i => i.transform.localPosition.y <= localMousePosition.y);

            if (under != null)
            {
                FriendsDivider.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
            }
            else
            {
                FriendsDivider.SetSiblingIndex(FriendsContent.childCount);
            }
        }
        else
        {
            FriendsDivider.gameObject.SetActive(false);
        }
    }

    private void OnPartyItemDrop(FriendItemController friendItemController)
    {
        FriendsDivider.gameObject.SetActive(false);
        
        Vector2 localMousePosition = FriendsContent.InverseTransformPoint(Input.mousePosition);

        if (FriendsContent.rect.Contains(localMousePosition))
        {
            friendItemController.transform.SetParent(FriendsContent);

            var under = FriendItems.FirstOrDefault(i => i.transform.localPosition.y <= localMousePosition.y);

            if (under != null)
            {
                friendItemController.transform.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
            }

            PartyItems.Remove(friendItemController);
            FriendItems.Add(friendItemController);
            SaveChanges();
            return;
        }

        friendItemController.ResetPosition();
    }

    private void OnFriendListHover()
    {
        Vector2 localMousePosition = PartyContent.InverseTransformPoint(Input.mousePosition);

        if (PartyContent.rect.Contains(localMousePosition))
        {
            PartyDivider.gameObject.SetActive(true);
            
            var under = PartyItems.FirstOrDefault(i => i.transform.localPosition.y <= localMousePosition.y);

            if (under != null)
            {
                PartyDivider.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
            }
            else
            {
                PartyDivider.SetSiblingIndex(PartyContent.childCount);
            }
        }
        else
        {
            PartyDivider.gameObject.SetActive(false);
        }
    }

    private void OnFriendListItemDrop(FriendItemController friendItemController)
    {
        PartyDivider.gameObject.SetActive(false);
        
        Vector2 localMousePosition = PartyContent.InverseTransformPoint(Input.mousePosition);

        if (PartyContent.rect.Contains(localMousePosition))
        {
            friendItemController.transform.SetParent(PartyContent);

            var under = PartyItems.FirstOrDefault(i => i.transform.localPosition.y <= localMousePosition.y);

            if (under != null)
            {
                friendItemController.transform.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
            }

            FriendItems.Remove(friendItemController);
            PartyItems.Add(friendItemController);
            SaveChanges();
            return;
        }

        friendItemController.ResetPosition();
    }
}
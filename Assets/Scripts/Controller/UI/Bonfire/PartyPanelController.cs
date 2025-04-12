using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyPanelController : MonoBehaviour, IBonfirePanel
{
    [field: SerializeField] private FriendItemController FriendItemPrefab { get; set; }
    [field: SerializeField] private RectTransform Content { get; set; }
    [field: SerializeField] private Transform PartyDivider { get; set; }

    private PartyManager PartyManager { get; set; }
    private BonfireScene BonfireScene { get; set; }
    private List<FriendItemController> PartyItems { get; set; }

    public List<BasePawn> Party => PartyItems.Select(f => f.Pawn).ToList();

    public void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        PartyManager = partyManager;
        BonfireScene = bonfireScene;
        PartyItems = new List<FriendItemController>();

        var availableParty = Application.Instance.GetManager<GameSaveManager>().GetAvailableParty();
        
        foreach (var pawnController in PartyManager.Party)
        {
            var pawnData = availableParty.First(p => pawnController.Pawn.Id == p.Id);
            var item = Instantiate(FriendItemPrefab, Content)
                .Init(pawnData, bonfireScene, this, FriendItemState.Active);
            PartyItems.Add(item);
        }

        PartyDivider.gameObject.SetActive(false);
    }

    public void OnClick(FriendItemController friendItemController)
    {
        BonfireScene.Select(friendItemController.Pawn);
    }

    public void OnPick(FriendItemController friendItemController)
    {
        BonfireScene.Select(friendItemController.Pawn);

        PartyDivider.gameObject.SetActive(true);

        PartyItems.Remove(friendItemController);
        friendItemController.transform.SetParent(transform.parent);

        if (PartyItems.Count == 1)
        {
            PartyItems[0].ChangeState(FriendItemState.Inactive);
        }
    }

    public void OnHover(FriendItemController friendItemController)
    {
        var under = PartyItems.FirstOrDefault(i => i.transform.position.y <= Input.mousePosition.y);
        PartyDivider.SetSiblingIndex(under != null
            ? Mathf.Clamp(under.transform.GetSiblingIndex() - 1, 0, PartyItems.Count)
            : PartyItems.Count);
    }

    public bool CanDrop()
    {
        return PartyItems.Count < PartyManager.PartySizeLimit;
    }

    public void OnDrop(FriendItemController friendItemController)
    {
        PartyDivider.gameObject.SetActive(false);

        var under = PartyItems.FirstOrDefault(i => i.transform.position.y <= Input.mousePosition.y);
        var index = under != null
            ? Mathf.Clamp(under.transform.GetSiblingIndex() - 1, 0, PartyItems.Count)
            : PartyItems.Count;

        friendItemController.transform.SetParent(Content);
        friendItemController.transform.SetSiblingIndex(index);

        PartyItems.Insert(index, friendItemController);

        BonfireScene.SaveChanges();

        if (PartyItems.Count > 1)
        {
            PartyItems.FirstOrDefault(i => i.State == FriendItemState.Inactive)?.ChangeState(FriendItemState.Active);
        }

        BonfireScene.Unselect();
        BonfireScene.UpdateArchetypes();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BonfireScene.BonfirePanel = this;
        PartyDivider.gameObject.SetActive(BonfireScene.IsDragging);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BonfireScene.BonfirePanel = null;
        PartyDivider.gameObject.SetActive(false);
    }
}
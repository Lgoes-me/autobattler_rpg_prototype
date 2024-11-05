using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyPanelController : MonoBehaviour, IBonfirePanel, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] private FriendItemController FriendItemPrefab { get; set; }
    
    [field: SerializeField] private RectTransform PartyContent { get; set; }

    [field: SerializeField] private Transform PartyDivider { get; set; }

    private PartyManager PartyManager { get; set; }
    private BonfireScene BonfireScene { get; set; }
    private List<FriendItemController> PartyItems { get; set; }
    public List<PawnData> Party => PartyContent.GetComponentsInChildren<FriendItemController>().Select(f => f.PawnData).ToList();

    public void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        PartyItems = new List<FriendItemController>();
        PartyManager = partyManager;
        BonfireScene = bonfireScene;
        
        foreach (var pawnController in partyManager.Party)
        {
            var pawnData = partyManager.AvailableParty.First(p => pawnController.Pawn.Id == p.Id);
            PartyItems.Add(Instantiate(FriendItemPrefab, PartyContent).Init(pawnData, bonfireScene, this, FriendItemState.Active));
        }
        
        PartyDivider.gameObject.SetActive(false);
    }

    public void OnPick(FriendItemController friendItemController)
    {
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

        if (under != null)
        {
            PartyDivider.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
        }
        else
        {
            PartyDivider.SetSiblingIndex(PartyContent.childCount);
        }
    }

    public void OnDrop(FriendItemController friendItemController)
    {
        PartyDivider.gameObject.SetActive(false);
        
        friendItemController.transform.SetParent(PartyContent);

        var under = PartyItems.FirstOrDefault(i => i.transform.position.y <= Input.mousePosition.y);

        if (under != null)
        {
            friendItemController.transform.SetSiblingIndex(under.transform.GetSiblingIndex() - 1);
        }

        PartyItems.Add(friendItemController);
        BonfireScene.SaveChanges();
            
        if (PartyItems.Count > 1)
        {
            PartyItems[0].ChangeState(FriendItemState.Active);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PartyDivider.gameObject.SetActive(BonfireScene.IsDragging);
        BonfireScene.BonfirePanel = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PartyDivider.gameObject.SetActive(false);
        BonfireScene.BonfirePanel = null;
    }
}

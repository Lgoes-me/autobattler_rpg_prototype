using UnityEngine;
using UnityEngine.EventSystems;

public class BaseFriendItemPanelController : MonoBehaviour, IBonfirePanel
{
    [field: SerializeField] protected FriendItemController FriendItemPrefab { get; private set; }
    [field: SerializeField] protected RectTransform Content { get; private set; }

    protected PartyManager PartyManager { get; private set; }
    protected BonfireScene BonfireScene { get; private set; }

    public virtual void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        PartyManager = partyManager;
        BonfireScene = bonfireScene;
    }

    public virtual void OnClick(FriendItemController friendItemController)
    {
        BonfireScene.Select(friendItemController.PawnData);
    }

    public virtual void OnPick(FriendItemController friendItemController)
    {
        BonfireScene.Select(friendItemController.PawnData);
    }

    public virtual void OnHover(FriendItemController friendItemController)
    {
        
    }

    public virtual void OnDrop(FriendItemController friendItemController)
    {
        BonfireScene.Unselect();
        BonfireScene.UpdateArchetypes();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        BonfireScene.BonfirePanel = this;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        BonfireScene.BonfirePanel = null;
    }

    public virtual bool CanDrop()
    {
        return true;
    }
}

public interface IBonfirePanel : IPointerEnterHandler, IPointerExitHandler
{
    void OnClick(FriendItemController friendItemController);
    void OnPick(FriendItemController friendItemController);
    void OnHover(FriendItemController friendItemController);
    bool CanDrop();
    void OnDrop(FriendItemController friendItemController);
}

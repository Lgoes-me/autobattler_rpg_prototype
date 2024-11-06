using UnityEngine.EventSystems;

public interface IBonfirePanel : IPointerEnterHandler, IPointerExitHandler
{
    void OnClick(FriendItemController friendItemController);
    void OnPick(FriendItemController friendItemController);
    void OnHover(FriendItemController friendItemController);
    bool CanDrop();
    void OnDrop(FriendItemController friendItemController);
}

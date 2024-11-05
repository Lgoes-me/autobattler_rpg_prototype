﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseFriendItemPanelController : MonoBehaviour, IBonfirePanel
{
    [field: SerializeField] protected FriendItemController FriendItemPrefab { get; private set; }
    [field: SerializeField] protected RectTransform Content { get; private set; }

    protected PartyManager PartyManager { get; private set; }
    protected BonfireScene BonfireScene { get; private set; }
    protected List<FriendItemController> FriendItems { get; private set; }

    public virtual void Init(PartyManager partyManager, BonfireScene bonfireScene)
    {
        FriendItems = new List<FriendItemController>();
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
        FriendItems.Remove(friendItemController);
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
}

public interface IBonfirePanel : IPointerEnterHandler, IPointerExitHandler
{
    void OnClick(FriendItemController friendItemController);
    void OnPick(FriendItemController friendItemController);
    void OnHover(FriendItemController friendItemController);
    void OnDrop(FriendItemController friendItemController);
}

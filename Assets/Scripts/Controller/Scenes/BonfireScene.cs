using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BonfireScene : BaseScene
{
    [field: SerializeField] private FriendsPanelController FriendsPanelController { get; set; }
    [field: SerializeField] private PartyPanelController PartyPanelController { get; set; }
    [field: SerializeField] private Button FinishButton { get; set; }

    private BonfireController BonfireController { get; set; }
    public IBonfirePanel BonfirePanel { get; set; }
    public bool IsDragging { get; set; }

    public void Init(BonfireController bonfireController)
    {
        BonfireController = bonfireController;
        FinishButton.onClick.AddListener(EndBonfireScene);

        var partyManager = Application.Instance.PartyManager;

        FriendsPanelController.Init(partyManager, this);
        PartyPanelController.Init(partyManager, this);
    }

    public void SaveChanges()
    {
        Application.Instance.PartyManager.SetSelectedParty(PartyPanelController.Party);
    }

    private void EndBonfireScene()
    {
        Application.Instance.SceneManager.EndBonfireScene();
        BonfireController.Preselect();
    }
}

public interface IBonfirePanel
{
    void OnPick(FriendItemController friendItemController);
    void OnHover(FriendItemController friendItemController);
    void OnDrop(FriendItemController friendItemController);
}

using UnityEngine;
using UnityEngine.UI;

public class BonfireScene : BaseScene
{
    [field: SerializeField] private FriendsPanelController FriendsPanelController { get; set; }
    [field: SerializeField] private PartyPanelController PartyPanelController { get; set; }
    [field: SerializeField] private ProfilePanel ProfilePanel { get; set; }
    [field: SerializeField] private Button FinishButton { get; set; }

    [field: SerializeField] private ArchetypeCanvasController ArchetypeCanvasPrefab { get; set; }
    [field: SerializeField] private Transform TeamInfoContent { get; set; }

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
        
        UpdateArchetypes();
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

    public void Select(BasePawn basePawn)
    {
        ProfilePanel.Select(basePawn);
    }

    public void Unselect()
    {
        ProfilePanel.Unselect();
    }

    public void UpdateArchetypes()
    {
        foreach (Transform archetypeCanvas in TeamInfoContent)
        {
            Destroy(archetypeCanvas.gameObject);
        }

        var archetypes = Application.Instance.PartyManager.Archetypes;
        
        foreach (var archetype in archetypes)
        {
            Instantiate(ArchetypeCanvasPrefab, TeamInfoContent).Init(archetype);
        }
    }
}
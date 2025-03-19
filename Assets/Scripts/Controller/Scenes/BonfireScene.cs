using System;
using UnityEngine;
using UnityEngine.UI;

public class BonfireScene : MonoBehaviour
{
    [field: SerializeField] private FriendsPanelController FriendsPanelController { get; set; }
    [field: SerializeField] private PartyPanelController PartyPanelController { get; set; }
    [field: SerializeField] private ProfilePanel ProfilePanel { get; set; }
    [field: SerializeField] private Button FinishButton { get; set; }

    [field: SerializeField] private ArchetypeCanvasController ArchetypeCanvasPrefab { get; set; }
    [field: SerializeField] private Transform TeamInfoContent { get; set; }

    private Action Callback { get; set; }
    public IBonfirePanel BonfirePanel { get; set; }
    public bool IsDragging { get; set; }

    public void Init(Action callback)
    {
        Callback = callback;
        FinishButton.onClick.AddListener(EndBonfireScene);

        var partyManager = Application.Instance.PartyManager;

        FriendsPanelController.Init(partyManager, this);
        PartyPanelController.Init(partyManager, this);
        
        Application.Instance.BlessingManager.ClearBlessings();
        
        UpdateArchetypes();
    }

    public void SaveChanges()
    {
        Application.Instance.PartyManager.SetSelectedParty(PartyPanelController.Party);
    }

    private void EndBonfireScene()
    {
        Application.Instance.SceneManager.EndBonfireScene();
        Callback();
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

        var archetypes = Application.Instance.ArchetypeManager.Archetypes;
        
        foreach (var archetype in archetypes)
        {
            Instantiate(ArchetypeCanvasPrefab, TeamInfoContent).Init(archetype);
        }
    }
}
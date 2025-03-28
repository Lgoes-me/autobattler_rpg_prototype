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
    
    private PartyManager PartyManager { get; set; }

    public void Init(Action callback)
    {
        Callback = callback;
        FinishButton.onClick.AddListener(EndBonfireScene);

        PartyManager = Application.Instance.GetManager<PartyManager>();

        FriendsPanelController.Init(PartyManager, this);
        PartyPanelController.Init(PartyManager, this);
        
        Application.Instance.GetManager<BlessingManager>().ClearBlessings();
        
        UpdateArchetypes();
    }

    public void SaveChanges()
    {
        PartyManager.SetSelectedParty(PartyPanelController.Party);
        var playerControllerPosition = Application.Instance.GetManager<PlayerManager>().PlayerController.transform.position;
        PartyManager.UnSpawnParty();
        PartyManager.SpawnPartyAt(playerControllerPosition);
    }

    private void EndBonfireScene()
    {
        Application.Instance.GetManager<SceneManager>().EndBonfireScene();
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

        var archetypes = Application.Instance.GetManager<ArchetypeManager>().Archetypes;
        
        foreach (var archetype in archetypes)
        {
            Instantiate(ArchetypeCanvasPrefab, TeamInfoContent).Init(archetype);
        }
    }
}
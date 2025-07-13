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

    public void Init(Spawn bonfireSpawn, Action callback)
    {
        Callback = callback;
        FinishButton.onClick.AddListener(EndBonfireScene);
        
        var gameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        gameSaveManager.ResetCurrentGameState(bonfireSpawn);

        FriendsPanelController.Init(gameSaveManager, this);
        PartyPanelController.Init(gameSaveManager, this);

        UpdateArchetypes();
    }

    public void SaveChanges()
    {
        var partyManager = Application.Instance.GetManager<PartyManager>();
        
        partyManager.SetSelectedParty(PartyPanelController.Party);
        
        partyManager.UnSpawnParty();
        partyManager.SpawnPartyAt(Application.Instance.GetManager<PlayerManager>().PlayerTransform.position);
    }

    private void EndBonfireScene()
    {
        SaveChanges();
        Application.Instance.GetManager<SceneManager>().EndBonfireScene();
        Callback();
    }

    public void Select(Pawn pawn)
    {
        ProfilePanel.Select(pawn);
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
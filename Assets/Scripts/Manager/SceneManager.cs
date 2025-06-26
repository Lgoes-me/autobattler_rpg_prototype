using System;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour, IManager
{
    [field: SerializeField] private CinemachineBlendDefinition Blend { get; set; }
    [field: SerializeField] private MapData MapData { get; set; }

    private bool BonfireActive { get; set; }

    private InterfaceManager InterfaceManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private PartyManager PartyManager { get; set; }

    private Map Map { get; set; }

    private RoomController CurrentRoom { get; set; }

    public void Prepare()
    {
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        PartyManager = Application.Instance.GetManager<PartyManager>();

        Map = MapData.ToDomain();
    }

    public void StartGameMenu()
    {
        var task = UnitySceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);

        task.completed += _ =>
        {
            GameSaveManager.LoadSave();
            var spawn = GameSaveManager.GetBonfireSpawn();
            Map.ChangeContext(spawn, VisualizeRoom);
        };
    }

    public void StartGameIntro()
    {
        Map.SpawnAt("Start", DoTransition);
    }

    public void GoToSpawn(string spawn)
    {
        Map.SpawnAt(spawn, DoTransition);
    }

    public void ChangeContext(Spawn spawn)
    {
        Map.ChangeContext(spawn, DoTransition);
    }

    private Task LoadNewRoom()
    {
        PartyManager.UnSpawnParty();

        if (UnitySceneManager.GetActiveScene().name == "RoomScene")
        {
            return Task.CompletedTask;
        }

        var tcs = new TaskCompletionSource<bool>();

        var task = UnitySceneManager.LoadSceneAsync("RoomScene", LoadSceneMode.Single);

        task.completed += _ => { tcs.SetResult(true); };

        return tcs.Task;
    }

    private void VisualizeRoom(BaseSceneNode sceneNode, Spawn spawn)
    {
        if (sceneNode is not SceneNode sceneData)
            return;

        Instantiate(sceneData.RoomPrefab).Init(sceneData);
    }

    private void DoTransition(BaseSceneNode baseSceneNode, Spawn spawn)
    {
        switch (baseSceneNode)
        {
            case SceneNode sceneNode:
            {
                EnterRoom(sceneNode, spawn);
                break;
            }
            case CutsceneNode cutsceneNode:
            {
                WatchCutscene(cutsceneNode, spawn);
                break;
            }
        }
    }

    private async void EnterRoom(SceneNode sceneNode, Spawn spawn)
    {
        await LoadNewRoom();

        if (CurrentRoom != null)
        {
            Destroy(CurrentRoom.gameObject);
        }

        await this.WaitEndOfFrame();

        CurrentRoom = Instantiate(sceneNode.RoomPrefab).Init(sceneNode);
        CurrentRoom.SpawnPlayerAt(spawn.Id, Blend);
        GameSaveManager.SetSpawn(spawn);

        PartyManager.SetPartyToFollow(true);
        CurrentRoom.PlayMusic();

        InterfaceManager.ShowBattleCanvas();
    }

    private void WatchCutscene(CutsceneNode cutsceneNode, Spawn spawn)
    {
        
    }
    
    public async void RespawnAtBonfire()
    {
        foreach (var pawn in PartyManager.Party)
        {
            pawn.EndBattle();
        }

        if (CurrentRoom != null)
        {
            Destroy(CurrentRoom.gameObject);
        }

        await this.WaitEndOfFrame();

        await LoadNewRoom();

        var spawn = GameSaveManager.GetBonfireSpawn();

        Map.ChangeContext(spawn, DoTransition);
        StartBonfireScene(spawn, () => { });
    }

    public void StartBonfireScene(Spawn bonfireSpawn, Action callback)
    {
        if (BonfireActive)
            return;

        BonfireActive = true;

        var task = UnitySceneManager.LoadSceneAsync("BonfireScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            PartyManager.StopPartyFollow();
            InterfaceManager.HideBattleCanvas();
            GameSaveManager.SetBonfireSpawn(bonfireSpawn);

            FindFirstObjectByType<BonfireScene>().Init(callback);
        };
    }

    public void EndBonfireScene()
    {
        if (!BonfireActive)
            return;

        BonfireActive = false;

        var task = UnitySceneManager.UnloadSceneAsync("BonfireScene");

        task.completed += _ =>
        {
            PartyManager.SetPartyToFollow(false);
            InterfaceManager.ShowBattleCanvas();
        };
    }

    public bool IsOpen(Transition transition)
    {
        return Map.IsOpen(transition);
    }

    public DialogueData GetDialogue(Transition transition)
    {
        return Map.GetDialogue(transition);
    }
}
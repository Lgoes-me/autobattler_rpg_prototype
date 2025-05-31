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

        Map = MapData.ToDomain(this);
    }

    public void StartGameMenu()
    {
        var task = UnitySceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
        
        task.completed += _ =>
        {
            GameSaveManager.LoadSave();
            var spawn = GameSaveManager.GetBonfireSpawn();
            var sceneNode = Map.SceneNodeById[spawn.Scene];
            CurrentRoom = Instantiate(sceneNode.RoomPrefab).Init(sceneNode);
        };
    }

    public void StartGameIntro()
    {
        Map.SpawnAt("Start");
    }
    
    public void ChangeContext(SpawnDomain spawn)
    {
        Map.ChangeContext(spawn);
    }

    public Task LoadNewRoom()
    {
        PartyManager.UnSpawnParty();
        
        if (UnitySceneManager.GetActiveScene().name == "RoomScene")
        {
            return Task.CompletedTask;
        }
        
        var tcs = new TaskCompletionSource<bool>();
        
        var task = UnitySceneManager.LoadSceneAsync("RoomScene", LoadSceneMode.Single);
        
        task.completed += _ =>
        {
            tcs.SetResult(true);
        };

        return tcs.Task;
    }

    public async void EnterRoom(SceneNode sceneNode, SpawnDomain spawnDomain)
    {
        if (CurrentRoom != null)
        {
            Destroy(CurrentRoom.gameObject);
        }

        await this.WaitEndOfFrame();
        
        CurrentRoom = Instantiate(sceneNode.RoomPrefab).Init(sceneNode);
        CurrentRoom.SpawnPlayerAt(spawnDomain.Destiantion, Blend);
        GameSaveManager.SetSpawn(spawnDomain);
        
        PartyManager.SetPartyToFollow(true);
        CurrentRoom.PlayMusic();

        InterfaceManager.ShowBattleCanvas();
    }
    
    public void OpenCutscene(string sceneName)
    {
        PartyManager.UnSpawnParty();
        
        var task = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var cutsceneScene = FindObjectOfType<CutsceneScene>();
            cutsceneScene.Init();
        };
    }
    
    public async void RespawnAtBonfire()
    {
        foreach (var pawn in PartyManager.Party)
        {
            pawn.FinishBattle();
        }
        
        if (CurrentRoom != null)
        {
            Destroy(CurrentRoom.gameObject);
        }

        await this.WaitEndOfFrame();
        
        await LoadNewRoom();

        var spawn = GameSaveManager.GetBonfireSpawn();
        
        var sceneNode = Map.SceneNodeById[spawn.Scene];
            
        CurrentRoom = Instantiate(sceneNode.RoomPrefab).Init(sceneNode);
           
        CurrentRoom.SpawnPlayerAt(spawn.Destiantion, Blend);
            
        PartyManager.SetPartyToFollow(true);

        StartBonfireScene(spawn, () => { });
    }

    public void StartBonfireScene(SpawnDomain bonfireSpawn, Action callback)
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

            FindObjectOfType<BonfireScene>().Init(callback);
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
}
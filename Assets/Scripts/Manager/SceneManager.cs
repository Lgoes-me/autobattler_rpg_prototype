using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] private SceneGraphData SceneGraphData { get; set; }
    //[field: SerializeField] private DungeonData Dungeon { get; set; }
    private bool BonfireActive { get; set; }

    private InterfaceManager InterfaceManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private PlayerManager PlayerManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private AudioManager AudioManager { get; set; }
    
    [field: SerializeField] private SceneGraph Map { get; set; }

    public void Prepare()
    {
        InterfaceManager = Application.Instance.InterfaceManager;
        GameSaveManager = Application.Instance.GameSaveManager;
        PlayerManager = Application.Instance.PlayerManager;
        PartyManager = Application.Instance.PartyManager;
        BlessingManager = Application.Instance.BlessingManager;
        AudioManager = Application.Instance.AudioManager;

        Map = SceneGraphData.ToDomain(this);
    }

    public void StartGameMenu()
    {
        var task = UnitySceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<StartMenuScene>();
        };
    }

    public void StartGameIntro()
    {
        Map.SpawnAt("Start");
        
        //var dungeon = Dungeon.GenerateDungeon(); 
        /*var task = UnitySceneManager.LoadSceneAsync("DungeonCutscene", LoadSceneMode.Single);

        task.completed += _ =>
        {
            var cutsceneScene = FindObjectOfType<CutsceneScene>();
            cutsceneScene.Init();

        };*/
    }

    public void ChangeContext(SpawnDomain spawn)
    {
        Map.ChangeContext(spawn);
    }

    public void EnterDungeon(Dungeon dungeon)
    {
        /*PartyManager.StopPartyFollow();
        var task = UnitySceneManager.LoadSceneAsync("RoomScene", LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene();
            roomScene.SpawnPlayerAt(doorName);

            PartyManager.SetPartyToFollow(true);
            AudioManager.PlayMusic(roomScene.Music);

            InterfaceManager.ShowBattleCanvas();
            GameSaveManager.SetSpawn(doorName, sceneName);
        };*/
    }

    public Task LoadNewRoom()
    {
        var tcs = new TaskCompletionSource<bool>();
        
        PartyManager.StopPartyFollow();
        var task = UnitySceneManager.LoadSceneAsync("RoomScene", LoadSceneMode.Single);
        
        task.completed += _ =>
        {
            tcs.SetResult(true);
        };

        return tcs.Task;
    }
    
    public void EnterRoom(SceneNode sceneNode, SpawnDomain spawnDomain)
    {
        var roomScene = FindObjectOfType<RoomScene>();
        roomScene.ActivateRoomScene(sceneNode);
        roomScene.SpawnPlayerAtDoor(spawnDomain.SpawnId);

        PartyManager.SetPartyToFollow(true);
        AudioManager.PlayMusic(roomScene.Music);

        InterfaceManager.ShowBattleCanvas();
        GameSaveManager.SetSpawn(spawnDomain);
    }
    
    public void OpenCutscene(string sceneName)
    {
        PartyManager.StopPartyFollow();
        
        var task = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var cutsceneScene = FindObjectOfType<CutsceneScene>();
            cutsceneScene.Init();
        };
    }

    public void RespawnAtBonfire()
    {
        PlayerManager.PlayerToWorld();

        foreach (var pawn in PartyManager.Party)
        {
            pawn.FinishBattle();
        }

        var spawn = GameSaveManager.GetBonfireSpawn();

        var respawnTask = UnitySceneManager.LoadSceneAsync("RoomScene", LoadSceneMode.Single);

        respawnTask.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene(Map.SceneNodeById[spawn.SceneId]);
            roomScene.SpawnPlayerAtBonfire(spawn.SpawnId);

            PartyManager.SetPartyToFollow(true);
            AudioManager.PlayMusic(roomScene.Music);
        };
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
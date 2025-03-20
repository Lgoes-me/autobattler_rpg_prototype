using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] private MapData MapData { get; set; }

    private bool BonfireActive { get; set; }

    private InterfaceManager InterfaceManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private PlayerManager PlayerManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private AudioManager AudioManager { get; set; }
    
    private Map Map { get; set; }

    public void Prepare()
    {
        InterfaceManager = Application.Instance.InterfaceManager;
        GameSaveManager = Application.Instance.GameSaveManager;
        PlayerManager = Application.Instance.PlayerManager;
        PartyManager = Application.Instance.PartyManager;
        BlessingManager = Application.Instance.BlessingManager;
        AudioManager = Application.Instance.AudioManager;

        Map = MapData.ToDomain(this);
    }

    public void StartGameMenu()
    {
        UnitySceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
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
        var tcs = new TaskCompletionSource<bool>();

        PartyManager.StopPartyFollow();
        var task = UnitySceneManager.LoadSceneAsync("RoomScene", LoadSceneMode.Single);

        task.completed += _ => { tcs.SetResult(true); };

        return tcs.Task;
    }

    public void EnterRoom(SceneNode sceneNode, SpawnDomain spawnDomain)
    {
        var room = Instantiate(sceneNode.RoomPrefab).Init(sceneNode);
        room.SpawnPlayerAt(spawnDomain.SpawnId);
        
        PartyManager.SetPartyToFollow(true);
        room.PlayMusic();

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
        PlayerManager.EnablePlayerInput();

        foreach (var pawn in PartyManager.Party)
        {
            pawn.FinishBattle();
        }

        var spawn = GameSaveManager.GetBonfireSpawn();

        var respawnTask = UnitySceneManager.LoadSceneAsync("RoomScene", LoadSceneMode.Single);

        respawnTask.completed += _ =>
        {
            var sceneNode = Map.SceneNodeById[spawn.SceneId];
            
            var room = Instantiate(sceneNode.RoomPrefab).Init(sceneNode);
            room.SpawnPlayerAt(spawn.SpawnId);

            PartyManager.SetPartyToFollow(true);
            AudioManager.PlayMusic(sceneNode.Music);
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
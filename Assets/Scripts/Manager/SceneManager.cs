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
    private PlayerManager PlayerManager { get; set; }

    private Map Map { get; set; }

    private BaseRoomController CurrentRoom { get; set; }

    public void Prepare()
    {
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        PartyManager = Application.Instance.GetManager<PartyManager>();
        PlayerManager = Application.Instance.GetManager<PlayerManager>();

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

    public void GoToSpawn(string spawnKey)
    {
        Map.SpawnAt(spawnKey, (node, spawn) =>
        {
            GameSaveManager.ResetCurrentGameState(spawn);
            DoTransition(node, spawn);
        });
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

    private void VisualizeRoom(BaseSceneNode node, Spawn spawn)
    {
        Instantiate(node.Prefab);//.Init(node, spawn, Blend);
    }

    private async void DoTransition(BaseSceneNode node, Spawn spawn)
    {
        PlayerManager.DisablePlayerInput();
        
        await LoadNewRoom();

        if (CurrentRoom != null)
        {
            Destroy(CurrentRoom.gameObject);
        }

        await this.WaitEndOfFrame();

        CurrentRoom = Instantiate(node.Prefab).Init(node, spawn, Blend);
        
        PlayerManager.EnablePlayerInput();
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
            Application.Instance.GetManager<BlessingManager>().ClearBlessings();

            FindFirstObjectByType<BonfireScene>().Init(bonfireSpawn, callback);
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
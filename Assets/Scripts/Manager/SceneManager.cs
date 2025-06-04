using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
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
        Map.SpawnAt("Start", EnterRoom);
    }

    public void ChangeContext(Spawn spawn)
    {
        Map.ChangeContext(spawn, EnterRoom);
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

    private void VisualizeRoom(SceneData sceneData, Spawn spawn)
    {
        Instantiate(sceneData.RoomPrefab).Init(sceneData);
    }

    private async void EnterRoom(SceneData sceneData, Spawn spawn)
    {
        await LoadNewRoom();

        if (CurrentRoom != null)
        {
            Destroy(CurrentRoom.gameObject);
        }

        await this.WaitEndOfFrame();

        CurrentRoom = Instantiate(sceneData.RoomPrefab).Init(sceneData);
        CurrentRoom.SpawnPlayerAt(spawn.Id, Blend);
        GameSaveManager.SetSpawn(spawn);

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

        Map.ChangeContext(spawn, EnterRoom);
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

    public bool IsOpen(Transition transition)
    {
        return Map.IsOpen(transition);
    }
}

public class SceneData
{
    public string Id { get; }
    public RoomController RoomPrefab { get; }
    public List<Transition> Doors { get; }
    public MusicType Music { get; }
    public List<CombatEncounterData> CombatEncounters { get; }
    public VolumeProfile PostProcessProfile { get; }

    public SceneData(
        string id,
        RoomController roomPrefab,
        List<Transition> doors,
        MusicType music,
        List<CombatEncounterData> combatEncounters,
        VolumeProfile postProcessProfile)
    {
        Id = id;
        RoomPrefab = roomPrefab;
        Doors = doors;
        Music = music;
        CombatEncounters = combatEncounters;
        PostProcessProfile = postProcessProfile;
    }
}
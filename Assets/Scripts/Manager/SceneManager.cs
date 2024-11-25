using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    private bool BonfireActive { get; set; }

    private InterfaceManager InterfaceManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private PlayerManager PlayerManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private AudioManager AudioManager { get; set; }

    public void Prepare()
    {
        InterfaceManager = Application.Instance.InterfaceManager;
        GameSaveManager = Application.Instance.GameSaveManager;
        PlayerManager = Application.Instance.PlayerManager;
        PartyManager = Application.Instance.PartyManager;
        BlessingManager = Application.Instance.BlessingManager;
        AudioManager = Application.Instance.AudioManager;
    }

    public void StartGameMenu()
    {
        var task = UnitySceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);

        task.completed += _ =>
        {
            //var roomScene = FindObjectOfType<StartMenuScene>();
        };
    }

    public void StartGameIntro()
    {
        //Cutscene
        SpawnPlayerAtWord();
    }

    public void SpawnPlayerAtWord()
    {
        PartyManager.Init();
        BlessingManager.Init();

        var spawn = GameSaveManager.GetSpawn();

        var task = UnitySceneManager.LoadSceneAsync(spawn.SceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene();
            roomScene.SpawnPlayerAt(spawn.Id);

            PartyManager.SetPartyToFollow(true);
            AudioManager.PlayMusic(roomScene.Music);
            InterfaceManager.ShowBattleCanvas();
        };
    }

    public void UseDoorToChangeScene(SpawnDomain spawn)
    {
        PartyManager.StopPartyFollow();
        var task = UnitySceneManager.LoadSceneAsync(spawn.SceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene();
            roomScene.SpawnPlayerAt(spawn.Id);

            PartyManager.SetPartyToFollow(true);
            AudioManager.PlayMusic(roomScene.Music);

            GameSaveManager.SetSpawn(spawn);
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

        var respawnTask = UnitySceneManager.LoadSceneAsync(spawn.SceneName, LoadSceneMode.Single);

        respawnTask.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene();
            roomScene.SpawnPlayerAt(spawn.Id);

            PartyManager.SetPartyToFollow(true);
            AudioManager.PlayMusic(roomScene.Music);
        };
    }

    public void StartBonfireScene(BonfireController bonfireController, SpawnDomain bonfireSpawn)
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

            FindObjectOfType<BonfireScene>().Init(bonfireController);
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
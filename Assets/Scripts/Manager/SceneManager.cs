using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] private GameSaveManager GameSaveManager { get; set; }
    [field: SerializeField] private PlayerManager PlayerManager { get; set; }
    [field: SerializeField] private PartyManager PartyManager { get; set; }
    [field: SerializeField] private AudioManager AudioManager { get; set; }

    private bool BonfireActive { get; set; }

    public void StartGame()
    {
        var spawn = Application.Instance.GameSaveManager.GetSpawn();

        var task = UnitySceneManager.LoadSceneAsync(spawn.SceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene();
            roomScene.SpawnPlayerAt(spawn.Id);

            PartyManager.SetPartyToFollow(true);
            AudioManager.PlayMusic(roomScene.Music);
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

            Application.Instance.GameSaveManager.SetSpawn(spawn);
        };
    }

    public void RespawnAtBonfire()
    {
        PlayerManager.PlayerToWorld();

        foreach (var pawn in PartyManager.Party)
        {
            pawn.Deactivate();
        }

        var spawn = Application.Instance.GameSaveManager.GetBonfireSpawn();

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

    public void StartBonfireScene(SpawnDomain bonfireSpawn)
    {
        if (BonfireActive)
            return;

        var task = UnitySceneManager.LoadSceneAsync("BonfireScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            BonfireActive = true;
            FindObjectOfType<BonfireScene>().Init();
            PartyManager.StopPartyFollow();

            GameSaveManager.SetBonfireSpawn(bonfireSpawn);
        };
    }

    public void EndBonfireScene()
    {
        if (!BonfireActive)
            return;

        var task = UnitySceneManager.UnloadSceneAsync("BonfireScene");

        task.completed += _ =>
        {
            BonfireActive = false;
            PartyManager.SetPartyToFollow(false);
        };
    }
}
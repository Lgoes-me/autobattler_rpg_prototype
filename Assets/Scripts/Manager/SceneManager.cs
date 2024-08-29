using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] private PlayerManager PlayerManager { get; set; }


    public void StartGame()
    {
        var startingScene = "DungeonEntrance";

        var task = UnitySceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene("DungeonEntrance");
        };
    }

    public void UseDoorToChangeScene(string doorName, string sceneName)
    {
        var task = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene(doorName);
        };
    }

    public void StartBattleScene(string id, List<EnemyController> enemies)
    {
        PlayerManager.AddDefeated(id);

        var task = UnitySceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            PlayerManager.PlayerToBattle();
            var battleScene = FindObjectOfType<BattleScene>();
            battleScene.ActivateBattleScene(enemies);
        };
    }

    public void EndBattleScene()
    {
        var task = UnitySceneManager.UnloadSceneAsync("BattleScene");
        task.completed += _ =>
        {
            PlayerManager.PlayerToWorld();
        };
    }
}
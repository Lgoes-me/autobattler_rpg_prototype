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
            roomScene.ActivateRoomScene(this, PlayerManager.PlayerController,"DungeonEntrance");
        };
    }

    public void UseDoorToChangeScene(string doorName, string sceneName)
    {
        var task = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene(this, PlayerManager.PlayerController, doorName);
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
            battleScene.ActivateBattleScene(this, enemies);
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
    
    public void StartBonfireScene()
    {
        var task = UnitySceneManager.LoadSceneAsync("BonfireScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            
        };
    }

    public void EndBonfireScene()
    {
        var task = UnitySceneManager.UnloadSceneAsync("BonfireScene");
        
        task.completed += _ =>
        {
            
        };
    }
}
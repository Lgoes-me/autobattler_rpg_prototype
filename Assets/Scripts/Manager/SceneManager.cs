using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] private PlayerManager PlayerManager { get; set; }

    private bool BattleActive { get; set; }
    private bool BonfireActive { get; set; }
    
    public void StartGame()
    {
        var startingScene = "DungeonEntrance";

        var task = UnitySceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene(this, PlayerManager.PlayerController,"DungeonEntrance");
            Application.Instance.AudioManager.PlayMusic();
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
        if(BattleActive)
            return;
        
        PlayerManager.AddDefeated(id);

        var task = UnitySceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            PlayerManager.PlayerToBattle();
            var battleScene = FindObjectOfType<BattleScene>();
            battleScene.ActivateBattleScene(this, enemies);
            BattleActive = true;
        };
    }

    public void EndBattleScene()
    {
        if(!BattleActive)
            return;
        
        var task = UnitySceneManager.UnloadSceneAsync("BattleScene");
        
        task.completed += _ =>
        {
            PlayerManager.PlayerToWorld();
            BattleActive = false;
        };
    }
    
    public void StartBonfireScene()
    {
        if(BonfireActive)
            return;
        
        var task = UnitySceneManager.LoadSceneAsync("BonfireScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            BonfireActive = true;
        };
    }

    public void EndBonfireScene()
    {
        if(!BonfireActive)
            return;

        var task = UnitySceneManager.UnloadSceneAsync("BonfireScene");
        
        task.completed += _ =>
        {
            BonfireActive = false;
        };
    }
}
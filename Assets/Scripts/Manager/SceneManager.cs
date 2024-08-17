using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] private PlayerManager PlayerManager { get; set; }

    public void StartGame()
    {
        //var startingScene = "OpeningScene";
        var startingScene = "CityOutside";
        
        UnitySceneManager.LoadScene(startingScene, LoadSceneMode.Single);
    }
    
    public void UseDoorToChangeScene(string doorName, string sceneName)
    {
        var task = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var doors = FindObjectsByType<DoorController>(FindObjectsSortMode.None);
            var spawn = doors.First(d => d.DoorName == doorName).SpawnPoint;

            PlayerManager.SpawnPlayerAt(spawn);
        };
    }
    
    public void StartBattleScene(PlayerController player, string id, List<EnemyController> enemies)
    {
        PlayerManager.AddDefeated(id);
        
        var task = UnitySceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
        
        task.completed += _ =>
        {
            var arena = FindObjectOfType<ArenaController>();
            arena.Init(player, enemies);
        };
    }
    
    public void EndBattleScene()
    {
        var task = UnitySceneManager.UnloadSceneAsync("BattleScene");
        
        task.completed += _ =>
        {
            PlayerManager.SpawnPlayer();
        };
    }
}
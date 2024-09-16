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
        var save = Application.Instance.Save;
        var task = UnitySceneManager.LoadSceneAsync(save.Room, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            
            roomScene.ActivateRoomScene(this, PlayerManager.PlayerController,save.Door);
            
            Application.Instance.PartyManager.SetPartyToFollow(true);
            Application.Instance.AudioManager.PlayMusic(roomScene.Music);
        };
    }

    public void UseDoorToChangeScene(string doorName, string sceneName)
    {
        Application.Instance.PartyManager.StopPartyFollow();
        var task = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene(this, PlayerManager.PlayerController, doorName);
            
            Application.Instance.PartyManager.SetPartyToFollow(true);
            Application.Instance.AudioManager.PlayMusic(roomScene.Music);

            var save = Application.Instance.Save;
            save.Room = sceneName;
            save.Door = doorName;
            Application.Instance.SaveManager.SaveData(save);
        };
    }

    public void StartBattleScene(string id, List<EnemyController> enemies)
    {
        if(BattleActive)
            return;

        var task = UnitySceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            PlayerManager.PlayerToBattle();
            var battleScene = FindObjectOfType<BattleScene>();
            battleScene.ActivateBattleScene(id,this, enemies);
            
            Application.Instance.AudioManager.PlayMusic(MusicType.Battle);
            
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
            var roomScene = FindObjectOfType<RoomScene>();
            Application.Instance.AudioManager.PlayMusic(roomScene.Music);
            
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
            var bonfireScene = FindObjectOfType<BonfireScene>();
            bonfireScene.Init();
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
            PlayerManager.ClearDefeated();
            Application.Instance.PartyManager.SetPartyToFollow(false);
        };
    }
}
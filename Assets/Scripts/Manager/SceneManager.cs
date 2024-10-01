using System.Collections.Generic;
using System.Linq;
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
        var task = UnitySceneManager.LoadSceneAsync(save.Spawn.SceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene();
            roomScene.SpawnPlayerAt(save.Spawn.SpawnId);
            
            Application.Instance.PartyManager.SetPartyToFollow(true);
            Application.Instance.AudioManager.PlayMusic(roomScene.Music);
        };
    }

    public void UseDoorToChangeScene(SpawnDomain spawn)
    {
        Application.Instance.PartyManager.StopPartyFollow();
        var task = UnitySceneManager.LoadSceneAsync(spawn.SceneName, LoadSceneMode.Single);

        task.completed += _ =>
        {
            var roomScene = FindObjectOfType<RoomScene>();
            roomScene.ActivateRoomScene();
            roomScene.SpawnPlayerAt(spawn.SpawnId);
            
            Application.Instance.PartyManager.SetPartyToFollow(true);
            Application.Instance.AudioManager.PlayMusic(roomScene.Music);

            var save = Application.Instance.Save;
            save.Spawn = spawn;
            Application.Instance.SaveManager.SaveData(save);
        };
    }

    public void StartBattleScene(string id, List<EnemyInfo> enemies)
    {
        if(BattleActive)
            return;

        var task = UnitySceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            PlayerManager.PlayerToBattle();
            FindObjectOfType<BattleScene>().ActivateBattleScene(id, enemies);
            
            Application.Instance.AudioManager.PlayMusic(MusicType.Battle);
            
            BattleActive = true;
        };
    }

    public void EndBattleScene(string battleId)
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
            
            var save = Application.Instance.Save;
            save.PlayerPawn = Application.Instance.PlayerManager.PawnController.Pawn.GetPawnInfo();
            save.SelectedParty = Application.Instance.PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.GetPawnInfo());
            save.DefeatedEnemies.Add(battleId);
            Application.Instance.SaveManager.SaveData(save);
        };
    }
    
    public void RespawnAtBonfire()
    {
        if(!BattleActive)
            return;
        
        var task = UnitySceneManager.UnloadSceneAsync("BattleScene");
        
        task.completed += _ =>
        {
            BattleActive = false;
            
            var save = Application.Instance.Save;
            var respawnTask = UnitySceneManager.LoadSceneAsync(save.LastBonfireSpawn.SceneName, LoadSceneMode.Single);

            respawnTask.completed += _ =>
            {
                PlayerManager.PlayerToWorld();
                var roomScene = FindObjectOfType<RoomScene>();
                roomScene.ActivateRoomScene();
                roomScene.SpawnPlayerAt(save.LastBonfireSpawn.SpawnId);
            
                Application.Instance.PartyManager.SetPartyToFollow(true);
                Application.Instance.AudioManager.PlayMusic(roomScene.Music);
                Application.Instance.SaveManager.SaveData(save);
            };
        };
    }
    
    public void StartBonfireScene(SpawnDomain bonfireSpawn)
    {
        if(BonfireActive)
            return;
        
        var task = UnitySceneManager.LoadSceneAsync("BonfireScene", LoadSceneMode.Additive);

        task.completed += _ =>
        {
            BonfireActive = true;
            FindObjectOfType<BonfireScene>().Init();
            Application.Instance.PartyManager.StopPartyFollow();
            
            var save = Application.Instance.Save;
            
            save.Spawn = bonfireSpawn;
            save.LastBonfireSpawn = bonfireSpawn;
            save.PlayerPawn = Application.Instance.PlayerManager.PawnController.PawnData.ResetPawnInfo();
            save.SelectedParty = Application.Instance.PartyManager.Party.ToDictionary(p => p.PawnData.Id, p => p.PawnData.ResetPawnInfo());
            save.DefeatedEnemies.Clear();
            
            Application.Instance.SaveManager.SaveData(save);
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
            Application.Instance.PartyManager.SetPartyToFollow(false);
        };
    }
}
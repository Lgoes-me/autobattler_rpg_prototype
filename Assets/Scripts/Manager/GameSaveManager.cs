using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    [field: SerializeField] private PlayerManager PlayerManager { get; set; }
    [field: SerializeField] private BlessingManager BlessingManager { get; set; }
    [field: SerializeField] private PartyManager PartyManager { get; set; }
    [field: SerializeField] private SaveManager SaveManager { get; set; }
    [field: SerializeField] private PawnData PlayerStartingCharacter { get; set; }

    private Save Save { get; set; }

    public void Init()
    {
        Save = SaveManager.LoadData<Save>("Save.json") ?? new Save(PlayerStartingCharacter);
    }

    public SpawnDomain GetSpawn()
    {
        return Save.Spawn;
    }

    public void SetSpawn(SpawnDomain spawn)
    {
        Save.Spawn = spawn;
        SaveManager.SaveData(Save);
    }

    public SpawnDomain GetBonfireSpawn()
    {
        return Save.LastBonfireSpawn;
    }

    public void SetBonfireSpawn(SpawnDomain spawn)
    {
        Save.Spawn = spawn;
        Save.LastBonfireSpawn = spawn;
        Save.PlayerPawn = PlayerManager.PawnController.Pawn.ResetPawnInfo();
        Save.SelectedParty = PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.ResetPawnInfo());
        Save.DefeatedEnemies.Clear();

        SaveManager.SaveData(Save);
    }
    
    public void SaveBattle(Battle battle)
    {
        Save.PlayerPawn = PlayerManager.PawnController.Pawn.GetPawnInfo();
        Save.SelectedParty = PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.GetPawnInfo());
        Save.DefeatedEnemies.Add(battle.Id);
        
        SaveManager.SaveData(Save);
    }
    
    public bool ContainsBattle(string battleId)
    {
        return Save.DefeatedEnemies.Contains(battleId);
    }
    
    public void SetParty(List<PawnData> newSelectedParty)
    {
        Save.SelectedParty = newSelectedParty.ToDictionary(p => p.Id, p => new PawnInfo(p.Id, 0));
        SaveManager.SaveData(Save);
    }

    public Dictionary<string, PawnInfo> GetSelectedParty()
    {
        return Save.SelectedParty;
    }

    public PawnInfo GetPlayerPawn()
    {
        return Save.PlayerPawn;
    }
    
    public void SetPlayerPawn(PawnData pawn)
    {
        Save.PlayerPawn = new PawnInfo(pawn.Id, 0);
        SaveManager.SaveData(Save);
    }

    public List<BlessingIdentifier> GetBlessings()
    {
        return Save.Blessings;
    }
    
    public void SetBlessings()
    {
        Save.Blessings = BlessingManager.Blessings.Select(j => j.Identifier).ToList();
        SaveManager.SaveData(Save);
    }

    public void ApplyPawnInfo(PawnDomain pawn)
    {
        if (Save.SelectedParty.TryGetValue(pawn.Id, out var pawnInfo))
        {
            pawn.SetPawnInfo(pawnInfo);
        }
        else if (Save.PlayerPawn.PawnName == pawn.Id)
        {
            pawn.SetPawnInfo(Save.PlayerPawn);
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    [field: SerializeField] private PawnData PlayerStartingCharacter { get; set; }

    private Save Save { get; set; }

    private ContentManager ContentManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private SaveManager SaveManager { get; set; }

    private void Start()
    {
        ContentManager = Application.Instance.ContentManager;
        BlessingManager = Application.Instance.BlessingManager;
        PartyManager = Application.Instance.PartyManager;
        SaveManager = Application.Instance.SaveManager;
    }

    public bool FirstTimePlaying()
    {
        return SaveManager.LoadList<Save>().Count == 0;
    }

    public void StartNewSave()
    {
        Save = new Save(PlayerStartingCharacter);
        SaveManager.SaveData(Save);
    }

    public void LoadSave()
    {
        Save = SaveManager.LoadList<Save>().First();
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
        Save.SelectedParty = PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.ResetPawnInfo());
        Save.DefeatedEnemies.Clear();

        SaveManager.SaveData(Save);
    }

    public void SaveBattle(Battle battle)
    {
        Save.SelectedParty = PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.GetPawnInfo());
        Save.DefeatedEnemies.Add(battle.Id);

        SaveManager.SaveData(Save);
    }

    public bool ContainsBattle(string battleId)
    {
        return Save.DefeatedEnemies.Contains(battleId);
    }

    public void SetParty(List<PawnFacade> newSelectedParty)
    {
        Save.SelectedParty = newSelectedParty.ToDictionary(p => p.Id, p => new PawnInfo(p.Id, 0));
        SaveManager.SaveData(Save);
    }

    public Dictionary<string, PawnInfo> GetSelectedParty()
    {
        return Save.SelectedParty;
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

    public void ApplyPawnInfo(Pawn pawn)
    {
        if (Save.SelectedParty.TryGetValue(pawn.Id, out var pawnInfo))
        {
            pawn.SetPawnInfo(pawnInfo);
        }
    }

    public List<PawnFacade> GetAvailableParty()
    {
        return Save.AvailableParty.Select(id => ContentManager.GetPawnFacadeFromId(id)).ToList();
    }

    public void AddToAvailableParty(PawnData pawnData)
    {
        Save.AvailableParty.Add(pawnData.Id);
        SaveManager.SaveData(Save);
    }

    public bool HasReadDialogue(string id)
    {
        return Save.Dialogues.Contains(id);
    }

    public void SaveDialogue(SkippableDialogue dialogue)
    {
        Save.Dialogues.Add(dialogue.Id);
        SaveManager.SaveData(Save);
    }
}
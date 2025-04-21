﻿using System;
using System.Collections.Generic;
using System.Linq;

public class GameSaveManager : IManager
{
    private Save Save { get; set; }

    private ContentManager ContentManager { get; set; }
    private TimeManager TimeManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private SaveManager SaveManager { get; set; }

    public void Prepare()
    {
        ContentManager = Application.Instance.GetManager<ContentManager>();
        TimeManager = Application.Instance.GetManager<TimeManager>();
        BlessingManager = Application.Instance.GetManager<BlessingManager>();
        PartyManager = Application.Instance.GetManager<PartyManager>();
        SaveManager = new SaveManager();
    }

    public bool FirstTimePlaying()
    {
        return SaveManager.LoadList<Save>().Where(l => l.LastBonfireSpawn != null).ToList().Count == 0;
    }

    public void StartNewSave()
    {
        Save = new Save().CreateNewSaveForIntro();

        var farmer = ContentManager.GetBasePawnFromId("Farmer");
        var hunter = ContentManager.GetBasePawnFromId("Hunter");

        AddToSelectedParty(
            new PawnInfo(farmer.Id, 1, 0, PawnStatus.Main, farmer.Weapon, farmer.Abilities));
        AddToSelectedParty(
            new PawnInfo(hunter.Id, 1, 0, PawnStatus.Unlocked, hunter.Weapon, hunter.Abilities));
    }

    public void AddToSelectedParty(PawnInfo pawnInfo)
    {
        Save.SelectedParty.Add(pawnInfo);
    }

    public void LoadSave()
    {
        Save = SaveManager.LoadList<Save>()
            .Where(l => l.LastBonfireSpawn != null)
            .OrderBy(l => DateTime.Now - l.Metadata.LastSaved)
            .First();
    }

    public SpawnDomain GetSpawn()
    {
        return Save.Spawn;
    }
    
    public void SetSpawn(SpawnDomain spawn)
    {
        Save.Spawn = spawn;
        SaveData();
    }
    
    public SpawnDomain GetBonfireSpawn()
    {
        return Save.LastBonfireSpawn;
    }

    public void SetBonfireSpawn(SpawnDomain spawn)
    {
        Save.Spawn = spawn;
        Save.LastBonfireSpawn = spawn;
        Save.SelectedParty = PartyManager.Party.Select(p => p.Pawn.ResetPawnInfo()).ToList();
        Save.DefeatedEnemies.Clear();

        SaveData();
    }

    public void SaveBattle(Battle battle)
    {
        Save.SelectedParty = PartyManager.Party.Select(p => p.Pawn.GetPawnInfo()).ToList();
        Save.DefeatedEnemies.Add(battle.Id);

        SaveData();
    }

    public bool ContainsBattle(string battleId)
    {
        return Save.DefeatedEnemies.Contains(battleId);
    }

    public void SetParty(List<BasePawn> newSelectedParty)
    {
        Save.SelectedParty = newSelectedParty
            .Select(p => new PawnInfo(p.Id, 1, 0, PawnStatus.Unlocked, p.Weapon, p.Abilities))
            .ToList();
        
        SaveData();
    }

    public List<PawnInfo> GetSelectedParty()
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
        
        SaveData();
    }

    public void ApplyPawnInfo(Pawn pawn)
    {
        var pawnInfo = Save.SelectedParty.First(p => p.Name == pawn.Id);
        pawn.SetPawnInfo(pawnInfo);
    }
    
    public void SavePawnInfo(PawnInfo updatedPawnInfo)
    {
        var pawnInfo = Save.SelectedParty.First(p => p.Name == updatedPawnInfo.Name);
        pawnInfo.Update(updatedPawnInfo);
        
        SaveData();
    }
    
    public List<BasePawn> GetAvailableParty()
    {
        return Save.AvailableParty.Select(id => ContentManager.GetBasePawnFromId(id)).ToList();
    }

    public void AddToAvailableParty(PawnData pawnData)
    {
        Save.AvailableParty.Add(pawnData.Id);
        
        SaveData();
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

    private void SaveData()
    {
        Save.CurrentTime = TimeManager.HorarioEmJogo;
        SaveManager.SaveData(Save);
    }

    public float GetSavedTime()
    {
        return Save.CurrentTime;
    }

    public void ClearParty()
    {
        Save.SelectedParty = Save.SelectedParty
        .Where(p => p.Status != PawnStatus.Transient)
        .ToList();

        SaveData();
    }

    public List<string> GetConsumables()
    {
        return Save.Consumables;
    }

    public void SetConsumables(List<ConsumableData> consumableDatas)
    {
        Save.Consumables = consumableDatas
            .Select(p => p.Id)
            .ToList();

        SaveData();
    }
}
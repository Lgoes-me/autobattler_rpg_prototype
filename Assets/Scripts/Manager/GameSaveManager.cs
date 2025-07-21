using System;
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

    public bool IsFirstTimePlaying()
    {
        return SaveManager.LoadList<Save>().Where(l => l!= null && l.LastBonfireSpawn != null).ToList().Count == 0;
    }

    public void StartNewSave()
    {
        var selectedParty = new List<PawnInfo>();
        
        var farmer = ContentManager.GetPawnFromId("Farmer");
        var hunter = ContentManager.GetPawnFromId("Hunter");

        selectedParty.Add(
            new PawnInfo(
                farmer.Id,
                1,
                0,
                0,
                PawnStatus.Main,
                farmer.GetComponent<WeaponComponent>().Weapon,
                farmer.GetComponent<AbilitiesComponent>().Abilities,
                farmer.GetComponent<ConsumableComponent>().Consumables,
                farmer.GetComponent<PawnBuffsComponent>().PermanentBuffs));


        selectedParty.Add(
            new PawnInfo(
                hunter.Id,
                1,
                0,
                0,
                PawnStatus.Unlocked,
                hunter.GetComponent<WeaponComponent>().Weapon,
                hunter.GetComponent<AbilitiesComponent>().Abilities,
                hunter.GetComponent<ConsumableComponent>().Consumables,
                hunter.GetComponent<PawnBuffsComponent>().PermanentBuffs));
        
        Save = new Save().CreateNewSaveForIntro(selectedParty);
    }

    public void LoadSave()
    {
        Save = SaveManager.LoadList<Save>()
            .Where(l => l.LastBonfireSpawn != null)
            .OrderBy(l => DateTime.Now - l.Metadata.LastSaved)
            .First();
    }
    
    //Get
    
    public Spawn GetSpawn()
    {
        return Save.Spawn;
    }

    public Spawn GetBonfireSpawn()
    {
        return Save.LastBonfireSpawn;
    }

    public List<PawnInfo> GetSelectedParty()
    {
        return Save.SelectedParty;
    }

    public List<string> GetBlessings()
    {
        return Save.Blessings;
    }

    public List<Pawn> GetAvailableParty()
    {
        return Save.AvailableParty.Select(id => ContentManager.GetPawnFromId(id)).ToList();
    }
    
    public PawnInfo GetPawnInfo(Pawn pawn)
    {
        return Save.SelectedParty.First(p => p.Name == pawn.Id);
    }

    public float GetSavedTime()
    {
        return Save.CurrentTime;
    }

    //GameState
    
    public void SaveCurrentGameState()
    {
        Save.Blessings = BlessingManager.Blessings.Select(j => j.name).ToList();
        Save.SelectedParty = PartyManager.Party.Select(p => p.Pawn.GetPawnInfo()).ToList();
        
        Save.CurrentTime = TimeManager.HorarioEmJogo;
        SaveManager.SaveData(Save);
    }

    public void ResetCurrentGameState(Spawn spawn)
    {
        Save.SelectedParty = PartyManager.Party.Select(p => p.Pawn.ResetPawnInfo()).ToList();
        
        Save.SelectedParty = Save.SelectedParty
            .Where(p => p.Status != PawnStatus.Transient)
            .ToList();

        Save.DefeatedEnemies.Clear();

        Save.Spawn = spawn;
        Save.LastBonfireSpawn = spawn;
        
        Save.CurrentTime = TimeManager.HorarioEmJogo;
        SaveManager.SaveData(Save);
    }

    //Checks
    
    public bool HasReadDialogue(string id)
    {
        return Save.Dialogues.Contains(id);
    }
    
    public bool ContainsEvent(string eventName)
    {
        return Save.Events.Contains(eventName);
    }
    
    public bool ContainsBattle(string battleId)
    {
        return Save.DefeatedEnemies.Contains(battleId);
    }

    //ADD

    public void AddBattle(Battle battle)
    {
        Save.DefeatedEnemies.Add(battle.Id);
    }
    
    public void AddToSelectedParty(PawnInfo pawnInfo)
    {
        Save.SelectedParty.Add(pawnInfo);
    }
    
    public void AddEvent(string eventName)
    {
        Save.Events.Add(eventName);
    }
    
    public void AddDialogue(SkippableDialogue dialogue)
    {
        Save.Dialogues.Add(dialogue.Id);
    }

    public void AddToAvailableParty(PawnData pawnData)
    {
        Save.AvailableParty.Add(pawnData.Id);
    }

    public void SetSpawn(Spawn spawn)
    {
        Save.Spawn = spawn;
    }

    public void SetParty(List<Pawn> newSelectedParty)
    {
        Save.SelectedParty = newSelectedParty
            .Select(p => new PawnInfo(
                p.Id,
                1,
                0,
                0,
                PawnStatus.Unlocked,
                p.GetComponent<WeaponComponent>().Weapon,
                p.GetComponent<AbilitiesComponent>().Abilities,
                p.GetComponent<ConsumableComponent>().Consumables,
                p.GetComponent<PawnBuffsComponent>().PermanentBuffs))
            .ToList();
    }
}
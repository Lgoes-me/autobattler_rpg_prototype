using System.Collections.Generic;
using System.Linq;

public class GameSaveManager
{
    private Save Save { get; set; }

    private ContentManager ContentManager { get; set; }
    private TimeManager TimeManager { get; set; }
    private BlessingManager BlessingManager { get; set; }
    private PartyManager PartyManager { get; set; }
    private SaveManager SaveManager { get; set; }

    public void Prepare()
    {
        ContentManager = Application.Instance.ContentManager;
        TimeManager = Application.Instance.TimeManager;
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
        Save = new Save();
        Save.CreateNewSaveForIntro();
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
        Save.SelectedParty = PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.ResetPawnInfo());
        Save.DefeatedEnemies.Clear();

        SaveData();
    }

    public void SaveBattle(Battle battle)
    {
        Save.SelectedParty = PartyManager.Party.ToDictionary(p => p.Pawn.Id, p => p.Pawn.GetPawnInfo());
        Save.DefeatedEnemies.Add(battle.Id);

        SaveData();
    }

    public bool ContainsBattle(string battleId)
    {
        return Save.DefeatedEnemies.Contains(battleId);
    }

    public void SetParty(List<BasePawn> newSelectedParty)
    {
        Save.SelectedParty = newSelectedParty.ToDictionary(p => p.Id, p => new PawnInfo(p.Id, 0));
        
        SaveData();
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
        
        SaveData();
    }

    public void ApplyPawnInfo(Pawn pawn)
    {
        if (Save.SelectedParty.TryGetValue(pawn.Id, out var pawnInfo))
        {
            pawn.SetPawnInfo(pawnInfo);
        }
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
}
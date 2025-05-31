using System;
using System.Collections.Generic;

[Serializable]
public class Save : ISavable
{
    public Metadata Metadata { get; set; }
    public Spawn Spawn { get; set; }
    public Spawn LastBonfireSpawn { get; set; }
    public List<PawnInfo> SelectedParty { get; set; }
    public List<string> DefeatedEnemies { get; set; }
    public List<BlessingIdentifier> Blessings { get; set; }
    public List<string> AvailableParty { get; set; }
    public List<string> Dialogues { get; set; }
    public float CurrentTime { get; set; }

    public Save()
    {
        Metadata = null;
        Spawn = null;
        LastBonfireSpawn = null;
        SelectedParty = new List<PawnInfo>();
        DefeatedEnemies = new List<string>();
        Blessings = new List<BlessingIdentifier>();
        AvailableParty = new List<string>();
        Dialogues = new List<string>();
        CurrentTime = 0f;
    }

    public Save CreateNewSaveForIntro(List<PawnInfo> selectedParty) 
    {
        Metadata = new Metadata().CreateNewDynamicFile(".json");
        
        Spawn = new Spawn(
            "1590e136-c0a0-4285-97cc-c066b6d353bf", 
            "5156dcbf-99fe-410b-b6c8-6e7496024057");
        
        LastBonfireSpawn = new Spawn(
            "efc187ae-edbb-4187-87a2-7be8298184c6", 
            "ed12450d-ce72-4d93-a724-c5a83dbead04");

        SelectedParty = selectedParty;

        DefeatedEnemies = new List<string>()
        {
            "Wolfs-2fbb78be-a5e0-4ec0-9d43-04bb71be0cc7",
            "Grunts-5561663c-72db-4b8e-b75e-09e085a009f1",
            "EnemyArea (2)-e366a721-bd24-4f71-adcd-83a7a465e7e2",
            "EnemyArea (1)-e366a721-bd24-4f71-adcd-83a7a465e7e2"
        };
        
        Blessings = new List<BlessingIdentifier>()
        {
            BlessingIdentifier.BattleStartGainMana,
            BlessingIdentifier.OnAttackHeal,
        };
        
        AvailableParty = new List<string>()
        {
            "Farmer",
            "Hunter",
        };
        
        Dialogues = new List<string>();
        
        CurrentTime = 3.9f;

        return this;
    }
}
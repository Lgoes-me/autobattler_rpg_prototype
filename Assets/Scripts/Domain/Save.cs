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
    public List<string> Blessings { get; set; }
    public List<string> AvailableParty { get; set; }
    public List<string> Dialogues { get; set; }
    public List<string> Events { get; set; }
    public float CurrentTime { get; set; }

    public Save()
    {
        Metadata = null;
        Spawn = null;
        LastBonfireSpawn = null;
        SelectedParty = new List<PawnInfo>();
        DefeatedEnemies = new List<string>();
        Blessings = new List<string>();
        AvailableParty = new List<string>();
        Dialogues = new List<string>();
        Events = new List<string>();
        CurrentTime = 0f;
    }

    public Save CreateNewSaveForIntro(List<PawnInfo> selectedParty) 
    {
        Metadata = new Metadata().CreateNewDynamicFile(".json");
        
        LastBonfireSpawn = new Spawn(
            "efc187ae-edbb-4187-87a2-7be8298184c6", 
            "e06d4e38-2c0e-4fb9-b250-934e7f7e22fc");

        SelectedParty = selectedParty;

        DefeatedEnemies = new List<string>();
        
        Blessings = new List<string>()
        {
            
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
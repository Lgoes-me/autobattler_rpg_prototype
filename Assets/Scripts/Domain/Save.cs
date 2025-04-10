using System;
using System.Collections.Generic;

[Serializable]
public class Save : ISavable
{
    public Metadata Metadata { get; set; }
    public SpawnDomain Spawn { get; set; }
    public SpawnDomain LastBonfireSpawn { get; set; }
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

    public Save CreateNewSaveForIntro()
    {
        Metadata = new Metadata().CreateNewDynamicFile(".json");
        Spawn = null;
        LastBonfireSpawn = null;
        
        SelectedParty = new List<PawnInfo>
        {
            new("Farmer"),
            new("Hunter"),
        };
        
        DefeatedEnemies = new List<string>();
        Blessings = new List<BlessingIdentifier>();
        AvailableParty = new List<string>();
        Dialogues = new List<string>();
        CurrentTime = 20f;

        return this;
    }
}
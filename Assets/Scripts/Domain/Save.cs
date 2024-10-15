﻿using System.Collections.Generic;

[System.Serializable]
public class Save : ISavable
{
    public string Id { get; set; }
    public SpawnDomain Spawn { get; set; }
    public SpawnDomain LastBonfireSpawn { get; set; }
    public PawnInfo PlayerPawn { get; set; }
    public Dictionary<string, PawnInfo> SelectedParty { get; set; }
    public List<string> DefeatedEnemies { get; set; }
    public List<JokerIdentifier> Jokers { get; set; }

    public Save()
    {
        Id = "Save.json";
        Spawn = null;
        LastBonfireSpawn = null;
        PlayerPawn = null;
        SelectedParty = new Dictionary<string, PawnInfo>();
        DefeatedEnemies = new List<string>();
        Jokers = new List<JokerIdentifier>();
    }

    public Save(PawnData startingPlayer)
    {
        Id = "Save.json";
        Spawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        LastBonfireSpawn = new SpawnDomain("DungeonEntrance", "DungeonEntrance");
        PlayerPawn = new PawnInfo(startingPlayer.name, 0); 
        SelectedParty = new Dictionary<string, PawnInfo>();
        DefeatedEnemies = new List<string>();
        Jokers = new List<JokerIdentifier>() {JokerIdentifier.BattleStartGainMana};
    }
}
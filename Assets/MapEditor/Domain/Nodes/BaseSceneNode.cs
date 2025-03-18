﻿using System.Collections.Generic;

public abstract class BaseSceneNode
{
    public string Id { get; protected set; }
    public string Name { get; protected set; }
    public List<DoorData> Doors { get; protected set; }
    public MusicType Music { get; private set; }

    protected BaseSceneNode()
    {
        Name = string.Empty;
        Id = string.Empty;
        Doors = new List<DoorData>();
        Music = MusicType.Dungeon;
    }
    
    protected BaseSceneNode(string name, string id, List<DoorData> doors) : this()
    {
        Name = name;
        Id = id;
        Doors = doors;
    }
}
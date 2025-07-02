
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CutsceneNodeData : BaseNodeData
{
    [field: SerializeField] public CutsceneRoomController CutsceneRoomPrefab { get; private set; }
    [field: SerializeField] private DialogueData DialogueData { get; set; }
    [field: SerializeField] private VolumeProfile PostProcessProfile { get; set; }
    [field: SerializeField] private MusicType Music { get; set; }

    public override void Init(NodeDataParams nodeDataParams)
    {
        var dataParams = (CutsceneNodeDataParams) nodeDataParams;

        Id = dataParams.Id;
        Name = name = dataParams.CutsceneRoomPrefab.name;
        CutsceneRoomPrefab = dataParams.CutsceneRoomPrefab;
        Doors = new List<DoorData>();
       
        var entrance = new DoorData
        {
            Name = "entrance",
            Id = Guid.NewGuid().ToString()
        };

        var exit = new DoorData
        {
            Name = "exit",
            Id = Guid.NewGuid().ToString()
        };
        
        Doors.Add(entrance);
        Doors.Add(exit);
    }

    public override BaseNode ToDomain()
    {
        return new CutsceneNode(
            Id, 
            Doors[0].ToDomain(Id), 
            Doors[1].ToDomain(Id), 
            CutsceneRoomPrefab, 
            DialogueData, 
            PostProcessProfile,
            Music);
    }
}

public class CutsceneNodeDataParams : NodeDataParams
{
    public string Id { get; }
    public CutsceneRoomController CutsceneRoomPrefab { get; }

    public CutsceneNodeDataParams(string id, CutsceneRoomController cutsceneRoomPrefab)
    {
        Id = id;
        CutsceneRoomPrefab = cutsceneRoomPrefab;
    }
}
using System;
using UnityEngine.Rendering;

public class CutsceneNode : BaseSceneNode
{
    public Transition Entrance { get; }
    public Transition Exit { get; }
    public CutsceneRoomController CutsceneRoomPrefab { get; }
    public DialogueData DialogueData { get; }
    public VolumeProfile PostProcessProfile { get; }
    public MusicType Music { get; }
    
    private CutsceneNode(string id) : base(id)
    {
        Entrance = null;
        Exit = null;
        CutsceneRoomPrefab = null;
        DialogueData = null;
        PostProcessProfile = null;
        Music = MusicType.Dungeon;
    }

    public CutsceneNode(
        string id, 
        Transition entrance, 
        Transition exit, 
        CutsceneRoomController cutsceneRoomPrefab, 
        DialogueData dialogueData,
        VolumeProfile postProcessProfile) : this(id)
    {
        Entrance = entrance;
        Exit = exit;
        CutsceneRoomPrefab = cutsceneRoomPrefab;
        DialogueData = dialogueData;
        PostProcessProfile = postProcessProfile;
    }

    public override void DoTransition(Map map, Spawn spawn, Action<BaseSceneNode, Spawn> callback)
    {
        callback(this, spawn);
    }

    public override bool IsOpen(Map map, Spawn spawn)
    {
        if (spawn == Entrance.Start)
        {
            return true;
        }
        if (spawn == Exit.Start)
        {
            return false;
        }
        
        return false;
    }
}
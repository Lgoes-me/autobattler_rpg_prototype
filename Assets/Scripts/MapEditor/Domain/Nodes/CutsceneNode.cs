using System;
using UnityEngine.Rendering;

public class CutsceneNode : BaseSceneNode
{
    public override BaseRoomController Prefab => CutsceneRoomPrefab;
    public DialogueData DialogueData { get; }
    public Transition Entrance { get; }
    public Transition Exit { get; }
    
    private CutsceneRoomController CutsceneRoomPrefab { get; }
    
    private CutsceneNode(
        VolumeProfile postProcessProfile, 
        MusicType music, 
        string id) : base(postProcessProfile, music, id)
    {
        Entrance = null;
        Exit = null;
        CutsceneRoomPrefab = null;
        DialogueData = null;
    }

    public CutsceneNode(
        string id, 
        Transition entrance, 
        Transition exit, 
        CutsceneRoomController cutsceneRoomPrefab, 
        DialogueData dialogueData,
        VolumeProfile postProcessProfile,
        MusicType music) : this(postProcessProfile, music, id)
    {
        Entrance = entrance;
        Exit = exit;
        CutsceneRoomPrefab = cutsceneRoomPrefab;
        DialogueData = dialogueData;
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
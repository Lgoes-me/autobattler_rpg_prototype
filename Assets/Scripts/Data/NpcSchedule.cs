using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcSchedule
{
    [field: SerializeField] public PawnData PawnData { get; set; }

    [field: SerializeField]
    [field: SerializeReference]
    public List<NpcPlacement> RoutinePlacement { get; set; }
}

[Serializable]
public abstract class NpcPlacement : IComponentData
{
    [field: SerializeField] public float Time { get; set; }

    public abstract void ControlCharacterController(NpcController npcController);
}

[Serializable]
public class SpawnNpcPlacement : NpcPlacement
{
    [field: SerializeField] private Transform SpawnPlacement { get; set; }
    [field: SerializeField] private DialogueData DialogueData { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        if (DialogueData != null)
        {
            npcController.WithDialogue(DialogueData);
        }

        npcController.transform.position = SpawnPlacement.position;
    }
}

[Serializable]
public class SpawnAndMoveNpcPlacement : NpcPlacement
{
    [field: SerializeField] private Transform SpawnPlacement { get; set; }
    [field: SerializeField] private Transform Destination { get; set; }
    [field: SerializeField] private DialogueData DialogueData { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        if (DialogueData != null)
        {
            npcController.WithDialogue(DialogueData);
        }

        npcController.transform.position = SpawnPlacement.position;
        npcController.WithPath(Destination, () => { });
    }
}

[Serializable]
public class MoveToNpcPlacement : NpcPlacement
{
    [field: SerializeField] private Transform DestinationPlacement { get; set; }
    [field: SerializeField] private DialogueData DialogueData { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        if (DialogueData != null)
        {
            npcController.WithDialogue(DialogueData);
        }

        npcController.WithPath(DestinationPlacement, () => { });
    }
}


[Serializable]
public class WaitAtNpcPlacement : NpcPlacement
{
    [field: SerializeField] private DialogueData DialogueData { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        if (DialogueData != null)
        {
            npcController.WithDialogue(DialogueData);
        }
    }
}

[Serializable]
public class DeSpawnNpcPlacement : NpcPlacement
{
    [field: SerializeField] private Transform DestinationPlacement { get; set; }
    [field: SerializeField] private DialogueData DialogueData { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        if (DialogueData != null)
        {
            npcController.WithDialogue(DialogueData);
        }
        
        npcController.WithPath(DestinationPlacement, npcController.DeSpawn);
    }
}
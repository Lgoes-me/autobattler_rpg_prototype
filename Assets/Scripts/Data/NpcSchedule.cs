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
    [field: SerializeField] private DialogueData DialogueData { get; set; }

    public NpcPlacement WithDialogue(NpcController npcController)
    {
        if (DialogueData != null)
        {
            npcController.WithDialogue(DialogueData);
        }

        return this;
    }
    
    public abstract void ControlCharacterController(NpcController npcController);
    public abstract void SpawnCharacterAt(NpcController npcController);
}

[Serializable]
public class SpawnAndMoveNpcPlacement : NpcPlacement
{
    [field: SerializeField] private Transform SpawnPlacement { get; set; }
    [field: SerializeField] private Transform Destination { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        npcController.transform.position = SpawnPlacement.position;
        npcController.WithPath(Destination, () => { });
    }
    
    public override void SpawnCharacterAt(NpcController npcController)
    {
        npcController.transform.position = Destination.position;
    }
}

[Serializable]
public class MoveToNpcPlacement : NpcPlacement
{
    [field: SerializeField] private Transform Destination { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        npcController.WithPath(Destination, () => { });
    }

    public override void SpawnCharacterAt(NpcController npcController)
    {
        npcController.transform.position = Destination.position;
    }
}

[Serializable]
public class DeSpawnNpcPlacement : NpcPlacement
{
    [field: SerializeField] private Transform Destination { get; set; }

    public override void ControlCharacterController(NpcController npcController)
    {
        npcController.WithPath(Destination, npcController.DeSpawn);
    }
    
    public override void SpawnCharacterAt(NpcController npcController)
    {
        npcController.DeSpawn();
    }
}

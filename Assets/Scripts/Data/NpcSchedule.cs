using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcSchedule
{
    [field: SerializeField] public PawnData PawnData { get; set; }
    [field: SerializeField] public List<NpcPlacementData> Routine { get; set; }
}

[Serializable]
public class NpcPlacementData
{
    [field: SerializeField] public Transform Placement { get; set; }
    [field: SerializeField] public float Time { get; set; }
    [field: SerializeField] public ScheduleEventType ScheduleEventType { get; set; }
    [field: SerializeField] public DialogueData DialogueData { get; set; }
}

public enum ScheduleEventType
{
    Spawn,
    WaitAt,
    Despawn
}
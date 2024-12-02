using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NpcSchedule : ScriptableObject
{
    [field: SerializeField] public PawnData PawnData { get; set; }
    [field: SerializeField] public List<NpcData> Routine { get; set; }
}

[Serializable]
public class NpcData
{
    [field: SerializeField] public SpawnData Spawn { get; set; }
    [field: SerializeField] public float Time { get; set; }
    [field: SerializeField] public DialogueData DialogueData { get; set; }
}

[Serializable]
public class SpawnData
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public string SceneName { get; set; }
}
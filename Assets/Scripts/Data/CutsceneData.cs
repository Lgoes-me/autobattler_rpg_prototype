using UnityEngine;

[CreateAssetMenu]
public class CutsceneData : ScriptableObject
{
    [field: SerializeField] private DialogueData Dialogue { get; set; }
    [field: SerializeReference] [field: SerializeField] private GameAction EndCutsceneAction { get; set; }
}
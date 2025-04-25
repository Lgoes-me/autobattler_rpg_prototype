using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnDataTeste : ScriptableObject
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeReference] [field: SerializeField] private List<PawnDataComponent> Components { get; set; }
}

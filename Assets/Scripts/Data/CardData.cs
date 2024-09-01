using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    [field: SerializeField] public PawnController Pawn { get; set; }
    [field: SerializeField] public PawnData PawnData { get; set; }
}

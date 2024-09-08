using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnController> SelectedParty { get; set; }
    [field: SerializeField] public List<PawnController> AvailableParty { get; set; }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject, IDialogue
{
    [field: SerializeReference] [field: SerializeField] public List<IDialogue> Lines { get; private set; }
}
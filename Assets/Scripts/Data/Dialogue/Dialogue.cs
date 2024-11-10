using System.Collections.Generic;
using UnityEngine;

public class Dialogue : IDialogue
{
    [field: SerializeReference] [field: SerializeField] public List<IDialogue> Lines { get; private set; }    
}
using UnityEngine;

[System.Serializable]
public class Line : IDialogue
{
    [field: SerializeField] public string Text { get; private set; }
}
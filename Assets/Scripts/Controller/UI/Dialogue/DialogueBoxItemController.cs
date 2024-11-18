using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxItemController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI TextMesh { get; set; }
    [field: SerializeField] private Button ContinueButton { get; set; }

    public void Init(string text, Action callback)
    {
        TextMesh.SetText(text);
        ContinueButton.onClick.AddListener(() => callback());
    }
}
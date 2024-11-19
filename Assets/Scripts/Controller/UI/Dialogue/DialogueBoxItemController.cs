using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxItemController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI TextMesh { get; set; }
    [field: SerializeField] private Button ContinueButton { get; set; }

    public bool CanContinue { get; private set; }
    
    public DialogueBoxItemController Init(Line line)
    {
        gameObject.SetActive(true);
        CanContinue = false;
        
        TextMesh.SetText(line.Text);
        ContinueButton.onClick.AddListener(() => CanContinue = true);

        return this;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        TextMesh.SetText("");
        ContinueButton.onClick.RemoveAllListeners();
    }
}
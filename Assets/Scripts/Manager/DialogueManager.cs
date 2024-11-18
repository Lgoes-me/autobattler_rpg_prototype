using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [field: SerializeField] private GameObject DialogueCanvas { get; set; }
    [field: SerializeField] private Transform DialogueContent { get; set; }
    [field: SerializeField] private DialogueBoxItemController DialogueBoxItemControllerPrefab { get; set; }

    public void OpenDialogue(DialogueData dialogueData)
    {
        StartCoroutine(Read(dialogueData));
    }

    private IEnumerator Read(DialogueData dialogueData)
    {
        DialogueCanvas.SetActive(true);
        yield return dialogueData.ReadDialogue(this);
        DialogueCanvas.SetActive(false);
    }

    public IEnumerator ShowText(string text)
    {
        var canContinue = false;
        Instantiate(DialogueBoxItemControllerPrefab, DialogueContent).Init(text, () => canContinue = true);
        yield return new WaitUntil(() => canContinue);
    }

    public IEnumerator ShowOptions(List<Option> options)
    {
        Option selectedOption = null;
        
        foreach (var option in options)
        {
            Instantiate(DialogueBoxItemControllerPrefab, DialogueContent).Init(option.Choice, () => selectedOption = option);
        }
        
        yield return new WaitUntil(() => selectedOption != null);
        
        yield return selectedOption?.ChooseOption(this);
    }
}
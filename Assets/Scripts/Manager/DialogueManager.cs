using System;
using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [field: SerializeField] private GameObject DialogueCanvas { get; set; }
    [field: SerializeField] private DialogueBoxItemController DialogueBoxItemController { get; set; }
    [field: SerializeField] private OptionBoxItemController OptionBoxItemController { get; set; }

    private Coroutine DialogueCoroutine { get; set; }
    
    public void OpenDialogue(DialogueData dialogueData, Action callback)
    {
        if (DialogueCoroutine != null)
        {
            StopCoroutine(DialogueCoroutine);
        }
        
        DialogueCoroutine = StartCoroutine(Read(dialogueData, callback));
    }
    
    private IEnumerator Read(DialogueData dialogueData, Action callback)
    {
        DialogueBoxItemController.Hide();
        OptionBoxItemController.Hide();
        
        DialogueCanvas.SetActive(true);
        
        yield return dialogueData.ReadDialogue(this);
        
        DialogueCanvas.SetActive(false);
        callback();
    }

    public IEnumerator ShowText(Line line, PawnData pawnData)
    {
        DialogueBoxItemController.Init(line, pawnData);
        yield return new WaitUntil(() => DialogueBoxItemController.CanContinue);
        DialogueBoxItemController.Hide();
    }

    public IEnumerator ShowOptions(DialogueOptions dialogueOptions, PawnData pawn)
    {
        OptionBoxItemController.Init(dialogueOptions);
        yield return new WaitUntil(() => OptionBoxItemController.SelectedOption != null);
        OptionBoxItemController.Hide();
        
        yield return OptionBoxItemController.SelectedOption?.ChooseOption(this, pawn);
    }

    public void CloseDialogue()
    {
        if (DialogueCoroutine != null)
        {
            StopCoroutine(DialogueCoroutine);
        }
        
        DialogueCanvas.SetActive(false);
    }
}
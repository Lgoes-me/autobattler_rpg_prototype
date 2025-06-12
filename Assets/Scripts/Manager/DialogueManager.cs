﻿using System;
using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IManager
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
        
        Application.Instance.GetManager<PauseManager>().PauseGame();
        Application.Instance.GetManager<PlayerManager>().DisablePlayerInput();
        
        DialogueCoroutine = StartCoroutine(Read(dialogueData, () =>
        {
            callback?.Invoke();
            Application.Instance.GetManager<PauseManager>().ResumeGame();
            Application.Instance.GetManager<PlayerManager>().EnablePlayerInput();
        }));
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

    public IEnumerator ShowText(Line line, Pawn pawn)
    {
        DialogueBoxItemController.Init(line, pawn);
        yield return new WaitUntil(() => DialogueBoxItemController.CanContinue);
        DialogueBoxItemController.Hide();
    }

    public IEnumerator ShowOptions(DialogueOptions dialogueOptions, Pawn pawn)
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
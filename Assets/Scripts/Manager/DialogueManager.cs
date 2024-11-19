using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [field: SerializeField] private GameObject DialogueCanvas { get; set; }
    [field: SerializeField] private DialogueBoxItemController DialogueBoxItemController { get; set; }
    [field: SerializeField] private OptionBoxItemController OptionBoxItemController { get; set; }

    private Coroutine DialogueCoroutine { get; set; }
    
    public void OpenDialogue(NpcController npcController, DialogueData dialogueData)
    {
        if (DialogueCoroutine != null)
        {
            StopCoroutine(DialogueCoroutine);
        }
        
        DialogueCoroutine = StartCoroutine(Read(npcController, dialogueData));
    }

    private IEnumerator Read(NpcController npcController, DialogueData dialogueData)
    {
        DialogueBoxItemController.Hide();
        OptionBoxItemController.Hide();
        
        DialogueCanvas.SetActive(true);
        
        yield return dialogueData.ReadDialogue(this);
        
        DialogueCanvas.SetActive(false);
        npcController.Preselect();
    }

    public IEnumerator ShowText(Line line)
    {
        DialogueBoxItemController.Init(line);
        yield return new WaitUntil(() => DialogueBoxItemController.CanContinue);
        DialogueBoxItemController.Hide();
    }

    public IEnumerator ShowOptions(DialogueOptions dialogueOptions)
    {
        OptionBoxItemController.Init(dialogueOptions);
        yield return new WaitUntil(() => OptionBoxItemController.SelectedOption != null);
        OptionBoxItemController.Hide();
        
        yield return OptionBoxItemController.SelectedOption?.ChooseOption(this);
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
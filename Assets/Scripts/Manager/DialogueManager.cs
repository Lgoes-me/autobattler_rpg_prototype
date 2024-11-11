using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public void OpenDialogue(DialogueData dialogueData)
    {
        StartCoroutine(Read(dialogueData));
    }

    private IEnumerator Read(DialogueData dialogueData)
    {
        yield return dialogueData.ReadDialogue(this);
    }

    public IEnumerator ShowText(string text)
    {
        Debug.Log(text);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator ShowOptions(List<Option> options)
    {
        foreach (var option in options)
        {
            Debug.Log(option.Choice);
        }

        var selectedOption = -1;

        yield return new WaitUntil(() =>
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedOption = 0;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedOption = 1;
                return true;
            }

            return false;
        });

        yield return options[selectedOption].ChooseOption(this);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionBoxItemController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI TextMesh { get; set; }
    
    [field: SerializeField] private Button FirstOption { get; set; }
    [field: SerializeField] private TextMeshProUGUI FirstOptionTextMesh { get; set; }
    [field: SerializeField] private Button SecondOption { get; set; }
    [field: SerializeField] private TextMeshProUGUI SecondOptionTextMesh { get; set; }

    public Option SelectedOption { get; private set; }
    
    public OptionBoxItemController Init(DialogueOptions dialogueOptions)
    {
        gameObject.SetActive(true);
        SelectedOption = null;
        
        TextMesh.SetText(dialogueOptions.Text);
        
        FirstOption.onClick.AddListener(() => SelectedOption = dialogueOptions.Options[0]);
        FirstOptionTextMesh.SetText(dialogueOptions.Options[0].Choice);
        
        SecondOption.onClick.AddListener(() => SelectedOption = dialogueOptions.Options[1]);
        SecondOptionTextMesh.SetText(dialogueOptions.Options[1].Choice);

        return this;
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        TextMesh.SetText("");
        FirstOption.onClick.RemoveAllListeners();
        FirstOptionTextMesh.SetText("");
        SecondOption.onClick.RemoveAllListeners();
        SecondOptionTextMesh.SetText("");
    }
}
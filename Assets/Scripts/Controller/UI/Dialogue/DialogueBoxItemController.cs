using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxItemController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI TextMesh { get; set; }
    [field: SerializeField] private Button ContinueButton { get; set; }
    [field: SerializeField] private Image Picture { get; set; }

    public bool CanContinue { get; private set; }
    
    public DialogueBoxItemController Init(Line line, BasePawn pawn)
    {
        gameObject.SetActive(true);
        CanContinue = false;
        
        TextMesh.SetText(line.Text);
        ContinueButton.onClick.AddListener(() => CanContinue = true);

        if (!string.IsNullOrWhiteSpace(line.Info))
        {
            var info = pawn.GetCharacterInfo(line.Info);
            
            Picture.sprite = info.Portrait;
            Application.Instance.GetManager<AudioManager>().PlaySfx(info.Audio);
        }

        return this;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        TextMesh.SetText("");
        ContinueButton.onClick.RemoveAllListeners();
    }
}
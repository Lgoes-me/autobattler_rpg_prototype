using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : MonoBehaviour
{
    [field: SerializeField] private BaseCanvasController CanvasController { get; set; }
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }
    [field: SerializeField] private Button SpecialButton { get; set; }
    
    public PawnController PawnController { get; private set; }

    public void Init(PawnController pawnController)
    {
        PawnController = pawnController;
        Show();
    }

    private void Show()
    {
        CanvasController.Show();
    }
    
    public void Hide()
    {
        CanvasController.Hide();
    }
    
    private void OnEnable()
    {
        var pawn = PawnController.Pawn;
        var fillAmount = pawn.Health / (float) pawn.MaxHealth;
        
        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;
        
        ManaBar.fillAmount = pawn.Mana / (float) pawn.MaxMana;
        
        SpecialButton.gameObject.SetActive(false);
        
        SpecialButton.onClick.AddListener(() =>
        {
            SpecialButton.gameObject.SetActive(false);
            PawnController.DoSpecial();
        });
    }

    public void UpdateLife(bool hideAfter)
    {
        var pawn = PawnController.Pawn;
        var fillAmount = pawn.Health / (float) pawn.MaxHealth;
        LifeBar.fillAmount = fillAmount;
        
        if(!gameObject.activeInHierarchy)
            return;
        
        StartCoroutine(UpdateBackgroundLifeBar(fillAmount, hideAfter));
    }

    private IEnumerator UpdateBackgroundLifeBar(float fillAmount, bool hideAfter)
    {
        yield return new WaitForSeconds(0.5f);
        BackgroundLifeBar.fillAmount = fillAmount;

        if (hideAfter)
            Hide();
    }

    public void UpdateMana(bool canActivateButton)
    {
        var pawn = PawnController.Pawn;
        ManaBar.fillAmount = pawn.Mana / (float) pawn.MaxMana;

        SpecialButton.gameObject.SetActive(canActivateButton && pawn.Mana == pawn.MaxMana);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : BaseCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }
    [field: SerializeField] private Button SpecialButton { get; set; }
    
    public bool Initiated { get; private set; }
    protected PawnController PawnController { get; private set; }
    
    public virtual void Init(PawnController pawnController)
    {
        Initiated = true;
        
        PawnController = pawnController;

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
        
        Show();
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
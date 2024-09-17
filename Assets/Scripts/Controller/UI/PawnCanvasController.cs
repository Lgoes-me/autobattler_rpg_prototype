using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : BaseCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }
    
    public bool Initiated { get; private set; }
    protected PawnController PawnController { get;  set; }
    
    public virtual void Init(PawnController pawnController)
    {
        Initiated = true;
        
        PawnController = pawnController;

        var pawn = PawnController.Pawn;
        var fillAmount = pawn.Health / (float) pawn.MaxHealth;
        
        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;
        
        ManaBar.fillAmount = pawn.Mana / (float) pawn.MaxMana;
        
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

    public virtual void UpdateMana()
    {
        var pawn = PawnController.Pawn;
        ManaBar.fillAmount = pawn.Mana / (float) pawn.MaxMana;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : BaseCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }
    
    public bool Initiated { get; protected set; }
    protected PawnController PawnController { get;  set; }
    
    public virtual void Init(PawnController pawnController)
    {
        Initiated = true;
        
        PawnController = pawnController;

        var pawn = PawnController.Pawn;
        var fillAmount = pawn.Stats.Health / (float) pawn.Stats.MaxHealth;
        
        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;
        
        ManaBar.fillAmount = pawn.Stats.Mana / (float) pawn.Stats.MaxMana;
        
        Show();
    }

    public void UpdateLife()
    {
        var pawn = PawnController.Pawn;
        var fillAmount = pawn.Stats.Health / (float) pawn.Stats.MaxHealth;
        LifeBar.fillAmount = fillAmount;
        
        if(!gameObject.activeInHierarchy)
            return;
        
        StartCoroutine(UpdateBackgroundLifeBar(fillAmount));
    }

    private IEnumerator UpdateBackgroundLifeBar(float fillAmount)
    {
        yield return new WaitForSeconds(0.5f);
        BackgroundLifeBar.fillAmount = fillAmount;

        if (fillAmount == 0)
            Death();
    }

    public virtual void UpdateMana()
    {
        var pawn = PawnController.Pawn;
        ManaBar.fillAmount = pawn.Stats.Mana / (float) pawn.Stats.MaxMana;
    }

    protected virtual void Death()
    {
        Hide(); 
    }
}
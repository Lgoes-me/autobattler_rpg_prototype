using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : BaseCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }
    protected PawnDomain Pawn { get; private set; }

    public virtual void Init(PawnDomain pawn)
    {
        Pawn = pawn;
        
        var fillAmount = Pawn.Health / (float) Pawn.MaxHealth;

        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;

        ManaBar.fillAmount = Pawn.HasMana ? Pawn.Mana / (float) Pawn.MaxMana : 0;

        Pawn.LifeChanged += UpdateLife;
        Pawn.ManaChanged += UpdateMana;
        
        Show();
    }

    private void UpdateLife()
    {
        var pawn = Pawn;
        var fillAmount = pawn.Health / (float) pawn.MaxHealth;
        LifeBar.fillAmount = fillAmount;

        if (!gameObject.activeInHierarchy)
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

    protected virtual void UpdateMana()
    {
        if (!Pawn.HasMana)
            return;
        
        ManaBar.fillAmount = Pawn.Mana / (float) Pawn.MaxMana;
    }
    
    protected void HideMana()
    {
        ManaBar.fillAmount = 0;
    }

    protected virtual void Death()
    {
        Pawn.LifeChanged -= UpdateLife;
        Pawn.ManaChanged -= UpdateMana;
        
        Hide();
    }
}
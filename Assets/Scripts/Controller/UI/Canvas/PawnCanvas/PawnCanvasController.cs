using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : BaseCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }
    protected Pawn Pawn { get; private set; }

    public virtual void Init(Pawn pawn)
    {
        Pawn = pawn;
        
        var fillAmount = Pawn.Health / (float) Pawn.MaxHealth;

        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;

        ManaBar.fillAmount = Pawn.HasMana ? Pawn.Mana / (float) Pawn.MaxMana : 0;
        
        Show();
    }
    
    public override void Show()
    {
        Pawn.LifeChanged += UpdateLife;
        Pawn.ManaChanged += UpdateMana;
        Pawn.BuffsChanged += UpdateBuffs;
        
        base.Show();
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
    
    protected virtual void UpdateBuffs()
    {
        
    }
    
    protected void HideMana()
    {
        ManaBar.fillAmount = 0;
    }

    protected virtual void Death()
    {
        Hide();
    }
    
    public override void Hide()
    {
        if (Pawn != null)
        {
            Pawn.LifeChanged -= UpdateLife;
            Pawn.ManaChanged -= UpdateMana;
            Pawn.BuffsChanged -= UpdateBuffs;
        }

        base.Hide();
    }
}
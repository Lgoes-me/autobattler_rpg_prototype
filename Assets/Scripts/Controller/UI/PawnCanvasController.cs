using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : BaseCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }

    public bool Initiated { get; protected set; }
    protected PawnDomain Pawn { get; private set; }

    public virtual void Init(PawnDomain pawn)
    {
        Pawn = pawn;
        Initiated = true;
        
        var fillAmount = Pawn.Health / (float) Pawn.MaxHealth;

        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;

        ManaBar.fillAmount = Pawn.HasMana ? Pawn.Mana / (float) Pawn.MaxMana : 0;

        Show();
    }

    public void UpdateLife()
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

    public virtual void UpdateMana()
    {
        if (!Pawn.HasMana)
            return;
        
        ManaBar.fillAmount = Pawn.Mana / (float) Pawn.MaxMana;
    }
    
    public virtual void HideMana()
    {
        ManaBar.fillAmount = 0;
    }

    protected virtual void Death()
    {
        Hide();
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarCanvasController : BasePawnCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }

    public override void Init(PawnController pawnController)
    {
        base.Init(pawnController);
        
        var fillAmount = Pawn.Health / (float) Pawn.GetPawnStats().Health;

        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;

        ManaBar.fillAmount = Pawn.HasMana ? Pawn.Mana / (float) Pawn.GetPawnStats().Mana : 0;

        Pawn.LostLife += UpdateLife;
        Pawn.GainedLife += UpdateLife;
        
        Pawn.LostMana += UpdateMana;
        Pawn.GainedMana += UpdateMana;
        
        Pawn.LostBuff += UpdateBuffs;
        Pawn.GainedBuff += UpdateBuffs;
    }

    private void UpdateLife()
    {
        var pawn = Pawn;
        var fillAmount = pawn.Health / (float) pawn.GetPawnStats().Health;
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

        ManaBar.fillAmount = Pawn.Mana / (float) Pawn.GetPawnStats().Mana;
    }

    protected virtual void UpdateBuffs()
    {
    }

    protected void HideMana()
    {
        ManaBar.fillAmount = 0;
    }

    protected override void Terminate()
    {
        base.Terminate();
        
        Pawn.LostLife -= UpdateLife;
        Pawn.GainedLife -= UpdateLife;
        
        Pawn.LostMana -= UpdateMana;
        Pawn.GainedMana -= UpdateMana;
        
        Pawn.LostBuff -= UpdateBuffs;
        Pawn.GainedBuff -= UpdateBuffs;
    }
}
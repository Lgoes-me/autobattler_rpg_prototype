using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarCanvasController : BasePawnCanvasController
{
    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }

    public override void Init(Pawn pawn)
    {
        base.Init(pawn);
        var stats = Pawn.GetComponent<StatsComponent>();
        var buffs = Pawn.GetComponent<PawnBuffsComponent>();
        
        var fillAmount = stats.Health / (float) stats.GetPawnStats().Health;

        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;

        ManaBar.fillAmount = stats.HasMana ? stats.Mana / (float) stats.GetPawnStats().Mana : 0;
        
        stats.LostLife += UpdateLife;
        stats.GainedLife += UpdateLife;
        stats.ManaChanged += UpdateMana;
        buffs.BuffsChanged += UpdateBuffs;
    }

    private void UpdateLife()
    {
        var stats = Pawn.GetComponent<StatsComponent>();
        var fillAmount = stats.Health / (float) stats.GetPawnStats().Health;
        
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

    private void UpdateMana()
    {
        var stats = Pawn.GetComponent<StatsComponent>();
        
        if (!stats.HasMana)
            return;

        ManaBar.fillAmount = stats.Mana / (float) stats.GetPawnStats().Mana;
    }

    protected virtual void UpdateBuffs()
    {
    }

    protected void HideMana()
    {
        ManaBar.fillAmount = 0;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        if(Pawn == null)
            return;
        
        var stats = Pawn.GetComponent<StatsComponent>();
        var buffs = Pawn.GetComponent<PawnBuffsComponent>();
        
        stats.LostLife -= UpdateLife;
        stats.GainedLife -= UpdateLife;
        
        stats.ManaChanged -= UpdateMana;
        buffs.BuffsChanged -= UpdateBuffs;
    }
}
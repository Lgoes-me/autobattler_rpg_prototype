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
        var resource = Pawn.GetComponent<ResourceComponent>();
        var buffs = Pawn.GetComponent<PawnBuffsComponent>();
        
        UpdateLife(0);
        UpdateMana(0);
        UpdateBuffs();

        resource.LostLife += ReceiveAttack;
        resource.GainedLife += UpdateLife;
        
        resource.LostMana += UpdateMana;
        resource.GainedMana += UpdateMana;
        
        buffs.BuffsChanged += UpdateBuffs;
    }

    private void ReceiveAttack(DamageDomain damageDomain)
    {
        UpdateLife(damageDomain.Value);
    }

    private void UpdateLife(int value)
    {
        var stats = Pawn.GetComponent<StatsComponent>();
        var resource = Pawn.GetComponent<ResourceComponent>();
        
        var fillAmount = resource.Health / (float) stats.GetStats().GetStat(Stat.Health);
        
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

    private void UpdateMana(int value)
    {
        var stats = Pawn.GetComponent<StatsComponent>();
        var resource = Pawn.GetComponent<ResourceComponent>();
        
        if (!resource.HasMana)
        {
            ManaBar.fillAmount = 0;
            return;
        }

        ManaBar.fillAmount = resource.Mana / (float) stats.GetStats().GetStat(Stat.Mana);
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
        
        var resource = Pawn.GetComponent<ResourceComponent>();
        var buffs = Pawn.GetComponent<PawnBuffsComponent>();
        
        resource.LostLife -= ReceiveAttack;
        resource.GainedLife -= UpdateLife;
        
        resource.GainedMana -= UpdateMana;
        resource.LostMana -= UpdateMana;
        
        buffs.BuffsChanged -= UpdateBuffs;
    }
}
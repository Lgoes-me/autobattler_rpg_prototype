using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCanvasController : LifeBarCanvasController
{
    [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField] private BuffsCanvasHolderController BuffsCanvasHolderController { get; set; }
    [field: SerializeField] private ConsumableCanvasHolderController ConsumableCanvasHolderController { get; set; }

    [field: SerializeField] private Image ProfilePicture { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }

    public override void Init(Pawn pawn)
    {
        base.Init(pawn);

        Name.SetText(Pawn.Id);
        CanvasGroup.alpha = 0.5f;
        
        Pawn.GetComponent<ConsumableComponent>().ConsumablesUpdated += UpdateConsumablesCanvas;
        
        UpdateProfile("default");
        UpdateConsumablesCanvas();
        Show();
    }

    protected override void StartBattle()
    {
        CanvasGroup.alpha = 1;

        ConsumableCanvasHolderController.StartBattle();
        
        UpdateProfile("battle");
    }

    protected override void FinishBattle()
    {
        CanvasGroup.alpha = 0.5f;
        
        UpdateProfile("default");
        
        ConsumableCanvasHolderController.FinishBattle();
        
        HideMana();
    }
    
    private void UpdateConsumablesCanvas()
    {
        var consumables = Pawn.GetComponent<ConsumableComponent>().Consumables.Select(c => new ConsumableCanvasControllerData(Pawn, c)).ToList();
        ConsumableCanvasHolderController.UpdateItems(consumables);
    }
    
    protected override void UpdateBuffs()
    {
        var buffs = Pawn.GetComponent<PawnBuffsComponent>().Buffs.Select(b => b.Value).ToList();
        BuffsCanvasHolderController.UpdateItems(buffs);
    }

    protected override void Death()
    {
        UpdateProfile("death");
        FinishBattle();
    }

    private void UpdateProfile(string identificador)
    {
        var info = Pawn.GetComponent<CharacterInfoComponent>().GetCharacterInfo(identificador);
            
        ProfilePicture.sprite = info.Portrait;
        Application.Instance.GetManager<AudioManager>().PlaySfx(info.Audio);
    }
    
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        if(Pawn == null)
            return;
        
        Pawn.GetComponent<ConsumableComponent>().ConsumablesUpdated -= UpdateConsumablesCanvas;
    }
}
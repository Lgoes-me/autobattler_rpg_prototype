using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCanvasController : LifeBarCanvasController
{
    [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField] private BuffItemController BuffItemPrefab { get; set; }
    [field: SerializeField] private ConsumableCanvasHolderController ConsumableCanvasHolderController { get; set; }

    [field: SerializeField] private Image ProfilePicture { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private Transform BuffsParent { get; set; }
    
    private List<BuffItemController> BuffItems { get; set; }

    public override void Init(Pawn pawn)
    {
        base.Init(pawn);

        BuffItems = new List<BuffItemController>();
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
        foreach (var item in BuffItems)
        {
            Destroy(item.gameObject);
        }

        BuffItems.Clear();

        var buffIdentifier = "default";
        var buffPriority = 0;
        
        var stats = Pawn.GetComponent<StatsComponent>();
        
        foreach (var (_, buff) in stats.Buffs)
        {
            if (buff.Priority > buffPriority)
            {
                buffIdentifier = buff.CharacterInfoIdentifier;
                buffPriority = buff.Priority;
            }
            
            BuffItems.Add(Instantiate(BuffItemPrefab, BuffsParent).Init(buff));
        }
        
        UpdateProfile(buffIdentifier);
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
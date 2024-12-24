using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCanvasController : LifeBarCanvasController
{
    [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField] private ButtonItemController SpecialButtonPrefab { get; set; }
    [field: SerializeField] private BuffItemController BuffItemPrefab { get; set; }
    
    [field: SerializeField] private Image ProfilePicture { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private Transform BuffsParent { get; set; }
    [field: SerializeField] private Transform SpecialButtonsParent { get; set; }

    private List<ButtonItemController> SpecialButtons { get; set; }
    private List<BuffItemController> BuffItems { get; set; }

    public override void Init(Pawn pawn)
    {
        base.Init(pawn);
        SpecialButtons = new List<ButtonItemController>();
        BuffItems = new List<BuffItemController>();
        Name.SetText(Pawn.Id);
        CanvasGroup.alpha = 0.5f;
        
        UpdateProfile("default");
        
        Show();
    }

    protected override void StartBattle(Battle battle)
    {
        CanvasGroup.alpha = 1;
        
        /*foreach (var ability in Pawn.SpecialAbilities)
        {
            SpecialButtons.Add(Instantiate(SpecialButtonPrefab, SpecialButtonsParent).Init(Pawn, ability, playerPawn, battle));
        }*/

        UpdateProfile("battle");
    }

    protected override void FinishBattle()
    {
        CanvasGroup.alpha = 0.5f;

        /*foreach (var specialButton in SpecialButtons)
        {
            Destroy(specialButton.gameObject);
        }*/

        UpdateProfile("default");
        
        //SpecialButtons.Clear();
        HideMana();
    }

    protected override void UpdateMana()
    {
        base.UpdateMana();

        foreach (var button in SpecialButtons)
        {
            button.TryActivateButton(Pawn.Mana);
        }
    }

    protected override void UpdateBuffs()
    {
        base.UpdateBuffs();

        foreach (var item in BuffItems)
        {
            Destroy(item.gameObject);
        }

        BuffItems.Clear();

        var buffIdentifier = "default";
        var buffPriority = 0;
        
        foreach (var (_, buff) in Pawn.Buffs)
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
        var info = Pawn.GetCharacterInfo(identificador);
            
        ProfilePicture.sprite = info.Portrait;
        Application.Instance.AudioManager.PlaySfx(info.Audio);
    }
}
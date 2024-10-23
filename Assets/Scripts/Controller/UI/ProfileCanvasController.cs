using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileCanvasController : PawnCanvasController
{
    [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField] private ButtonItemController SpecialButtonPrefab { get; set; }
    [field: SerializeField] private Transform SpecialButtonsParent { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }

    private List<ButtonItemController> SpecialButtons { get; set; }
    
    public override void Init(PawnDomain pawn)
    {
        base.Init(pawn);
        SpecialButtons = new List<ButtonItemController>();
        Name.SetText(Pawn.Id);
        CanvasGroup.alpha = 0.5f;
    }

    public void StartBattle()
    {
        CanvasGroup.alpha = 1;
        foreach (var ability in Pawn.SpecialAbilities)
        {
            SpecialButtons.Add(Instantiate(SpecialButtonPrefab, SpecialButtonsParent).Init(Pawn, ability));
        }
    }

    public void EndBattle()
    {
        CanvasGroup.alpha = 0.5f;
        
        foreach (var specialButton in SpecialButtons)
        {
            Destroy(specialButton.gameObject);
        }

        SpecialButtons.Clear();
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

    protected override void Death()
    {
        base.Death();
        EndBattle();
    }
}
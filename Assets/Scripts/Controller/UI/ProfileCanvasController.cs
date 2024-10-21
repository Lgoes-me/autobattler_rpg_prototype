using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileCanvasController : PawnCanvasController
{
    [field: SerializeField] private ButtonItemController SpecialButtonPrefab { get; set; }
    [field: SerializeField] private Transform SpecialButtonsParent { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }

    private List<ButtonItemController> SpecialButtons { get; set; }
    
    public override void Init(PawnDomain pawn)
    {
        base.Init(pawn);
        SpecialButtons = new List<ButtonItemController>();
        Name.SetText(Pawn.Id);

        foreach (var ability in Pawn.SpecialAbilities)
        {
            SpecialButtons.Add(Instantiate(SpecialButtonPrefab, SpecialButtonsParent).Init(Pawn, ability));
        }
    }

    public override void UpdateMana()
    {
        base.UpdateMana();
        
        foreach (var button in SpecialButtons)
        {
            button.TryActivateButton(Pawn.Mana);
        }
    }

    protected override void Death()
    {
        
    }

    public override void Hide()
    {
        foreach (var specialButton in SpecialButtons)
        {
            Destroy(specialButton.gameObject);
        }

        SpecialButtons.Clear();
        HideMana();
        
        Initiated = false;
    }
}
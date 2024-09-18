using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileCanvasController : PawnCanvasController
{
    [field: SerializeField] private ButtonItemController SpecialButtonPrefab { get; set; }
    [field: SerializeField] private Transform SpecialButtonsParent { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }

    private List<ButtonItemController> SpecialButtons { get; set; }
    
    public override void Init(PawnController pawnController)
    {
        base.Init(pawnController);
        SpecialButtons = new List<ButtonItemController>();
        Name.SetText(pawnController.PawnData.name);

        foreach (var ability in pawnController.Pawn.SpecialAttacks)
        {
            SpecialButtons.Add(Instantiate(SpecialButtonPrefab, SpecialButtonsParent).Init(pawnController, ability));
        }
    }

    public override void UpdateMana()
    {
        base.UpdateMana();

        var pawn = PawnController.Pawn;
        
        foreach (var button in SpecialButtons)
        {
            button.TryActivateButton(pawn.Mana);
        }
    }

    protected override void Death()
    {
        
    }
}
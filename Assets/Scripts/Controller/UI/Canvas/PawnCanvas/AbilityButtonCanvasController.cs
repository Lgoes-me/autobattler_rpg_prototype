using UnityEngine;

public class AbilityButtonCanvasController : BasePawnCanvasController
{
    [field: SerializeField] private ButtonItemController SpecialButton { get; set; }

    public override void Init(PawnController pawnController)
    {
        base.Init(pawnController);

        Pawn.ManaChanged += TryToActivateButton;
    }

    protected override void StartBattle(Battle battle)
    {
        base.StartBattle(battle);
        SpecialButton.Init(Pawn, Pawn.SpecialAbilities[0], PawnController, battle);
    }

    private void TryToActivateButton()
    {
        SpecialButton.TryActivateButton(Pawn.Mana);
    }

    protected override void Terminate()
    {
        base.Terminate();

        Pawn.ManaChanged -= TryToActivateButton;
    }
}
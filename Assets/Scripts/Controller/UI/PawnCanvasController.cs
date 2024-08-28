using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnController { get; set; }

    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }

    private void OnEnable()
    {
        UpdateLife();
        UpdateMana();
    }

    public void UpdateLife()
    {
        var pawn = PawnController.Pawn;
        LifeBar.fillAmount = pawn.Health / (float) pawn.MaxHealth;
    }

    public void UpdateMana()
    {
        var pawn = PawnController.Pawn;
        ManaBar.fillAmount = pawn.Mana / (float) pawn.MaxMana;
    }
}
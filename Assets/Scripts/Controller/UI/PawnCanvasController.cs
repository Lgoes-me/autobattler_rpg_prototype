using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnController { get; set; }

    [field: SerializeField] private Image LifeBar { get; set; }

    public void UpdateLife()
    {
        var pawn = PawnController.Pawn;
        LifeBar.fillAmount = pawn.Health / (float) pawn.MaxHealth;
    }
}
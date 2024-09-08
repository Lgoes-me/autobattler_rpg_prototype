using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PawnCanvasController : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnController { get; set; }

    [field: SerializeField] private Image LifeBar { get; set; }
    [field: SerializeField] private Image BackgroundLifeBar { get; set; }
    [field: SerializeField] private Image ManaBar { get; set; }
    [field: SerializeField] private Button SpecialButton { get; set; }

    private void OnEnable()
    {
        var pawn = PawnController.Pawn;
        var fillAmount = pawn.Health / (float) pawn.MaxHealth;
        
        LifeBar.fillAmount = fillAmount;
        BackgroundLifeBar.fillAmount = fillAmount;
        
        ManaBar.fillAmount = pawn.Mana / (float) pawn.MaxMana;
        
        SpecialButton.gameObject.SetActive(false);
        SpecialButton.onClick.AddListener(PawnController.DoSpecial);
    }

    public void UpdateLife(bool withAnimation)
    {
        var pawn = PawnController.Pawn;
        var fillAmount = pawn.Health / (float) pawn.MaxHealth;
        
        LifeBar.fillAmount = fillAmount;

        if (!withAnimation)
        {
            BackgroundLifeBar.fillAmount = fillAmount;
            return;
        }
        
        StartCoroutine(UpdateBackgroundLifeBar(fillAmount));
    }

    private IEnumerator UpdateBackgroundLifeBar(float fillAmount)
    {
        yield return new WaitForSeconds(0.5f);
        BackgroundLifeBar.fillAmount = fillAmount;
    }

    public void UpdateMana()
    {
        var pawn = PawnController.Pawn;
        ManaBar.fillAmount = pawn.Mana / (float) pawn.MaxMana;

        SpecialButton.gameObject.SetActive(pawn.Mana == pawn.MaxMana);
    }
}
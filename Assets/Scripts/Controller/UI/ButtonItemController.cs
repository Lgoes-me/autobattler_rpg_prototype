using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItemController : MonoBehaviour
{
    [field: SerializeField] private Button SpecialButton { get; set; }
    [field: SerializeField] private TextMeshProUGUI AbilityName { get; set; }
    [field: SerializeField] private TextMeshProUGUI AbilityCost { get; set; }
    
    private AbilityData Ability { get; set; }

    public ButtonItemController Init(Pawn pawn, AbilityData ability, PawnController playerPawn, Battle battle)
    {
        Ability = ability;
        
        AbilityName.SetText(ability.name);
        AbilityCost.SetText(ability.ResourceData.GetCost().ToString());

        SpecialButton.gameObject.SetActive(false);
        
        SpecialButton.onClick.AddListener(() =>
        {
            SpecialButton.gameObject.SetActive(false);
            pawn.DoSpecial(ability, playerPawn, battle);
        });
        
        return this;
    }

    public void TryActivateButton(int mana)
    {
        if(mana < Ability.ResourceData.GetCost())
            return;
        
        SpecialButton.gameObject.SetActive(true);
    }
}

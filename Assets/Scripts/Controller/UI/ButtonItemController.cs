using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItemController : MonoBehaviour
{
    [field: SerializeField] private Button SpecialButton { get; set; }
    [field: SerializeField] private TextMeshProUGUI AbilityName { get; set; }
    [field: SerializeField] private TextMeshProUGUI AbilityCost { get; set; }
    
    private AttackData Ability { get; set; }

    public ButtonItemController Init(PawnController pawnController, AttackData ability)
    {
        Ability = ability;
        
        AbilityName.SetText(ability.name);
        AbilityCost.SetText(ability.ManaCost.ToString());

        SpecialButton.gameObject.SetActive(false);
        
        SpecialButton.onClick.AddListener(() =>
        {
            SpecialButton.gameObject.SetActive(false);
            pawnController.DoSpecial(ability);
        });
        
        return this;
    }

    public void TryActivateButton(int mana)
    {
        if(mana < Ability.ManaCost)
            return;
        
        SpecialButton.gameObject.SetActive(true);
    }
}

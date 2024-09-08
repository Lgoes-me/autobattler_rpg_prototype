using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonfireScene : BaseScene
{
    [field: SerializeField] private TMP_Dropdown PlayerDropdown { get; set; }
    [field: SerializeField] private List<TMP_Dropdown> Dropdowns { get; set; }
    [field: SerializeField] private Button FinishButton { get; set; }

    private void Awake()
    {
        FinishButton.onClick.AddListener(EndBonfireScene);

        PlayerDropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new PawnOptionData(Application.Instance.PlayerManager.PawnController)
        };

        PlayerDropdown.interactable = false;
        
        var availableParty = new List<TMP_Dropdown.OptionData>()
        {
            new PawnOptionData(null)
        };
            
        availableParty.AddRange(Application.
                Instance.
                PartyManager.
                AvailableParty.
                Select(p => new PawnOptionData(p))
                .ToList<TMP_Dropdown.OptionData>());


        for (var index = 0; index < Dropdowns.Count; index++)
        {
            var dropdown = Dropdowns[index];
            dropdown.options = availableParty;

            if (Application.Instance.PartyManager.SelectedParty.Count > index)
            {
                var currentSelectedPawn = Application.Instance.PartyManager.SelectedParty[index];
                var selectedIndex = availableParty.FindIndex(o => ((PawnOptionData)o).PawnController == currentSelectedPawn);
                dropdown.value = selectedIndex;
            }
            else
            {
                dropdown.value = 0;
            }
            
            dropdown.onValueChanged.AddListener(SaveChanges);
        }
    }

    private void SaveChanges(int arg0)
    {
        var selectedPawns = new List<PawnController>();
        
        foreach (var dropdown in Dropdowns)
        {
            var pawnController = ((PawnOptionData) dropdown.options[dropdown.value]).PawnController;
            if(pawnController == null) continue;
            
            selectedPawns.Add(pawnController);
        }

        Application.Instance.PartyManager.SelectedParty = selectedPawns;
    }

    private void EndBonfireScene()
    {
        Application.Instance.SceneManager.EndBonfireScene();
    }
}


public class PawnOptionData : TMP_Dropdown.OptionData
{
    public PawnController PawnController;

    public PawnOptionData(PawnController pawnController) : base(pawnController?.PawnData.name ?? string.Empty)
    {
        PawnController = pawnController;
    }
}
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

    public void Init()
    {
        FinishButton.onClick.AddListener(EndBonfireScene);
        
        var availableParty = new List<TMP_Dropdown.OptionData>()
        {
            new PawnOptionData(null)
        };

        var pawns = Application.Instance.PartyManager.AvailableParty.Select(p => new PawnOptionData(p))
            .ToList<TMP_Dropdown.OptionData>();
        
        availableParty.AddRange(pawns);

        PlayerDropdown.options = pawns;
        var player = Application.Instance.PlayerManager.PawnController.Pawn;
        PlayerDropdown.value = availableParty.FindIndex(o => ((PawnOptionData)o).Pawn?.Id == player.Id) - 1;
        PlayerDropdown.onValueChanged.AddListener(SaveChangesToPlayer);

        for (var index = 0; index < Dropdowns.Count; index++)
        {
            var dropdown = Dropdowns[index];
            dropdown.options = availableParty;
            var selectedParty = Application.Instance.PartyManager.Party;

            if (selectedParty.Count > index)
            {
                var currentSelectedPawn = selectedParty[index];
                var selectedIndex = availableParty
                    .FindIndex(o => ((PawnOptionData)o).Pawn?.Id == currentSelectedPawn.Pawn.Id);
                dropdown.value = selectedIndex;
            }
            else
            {
                dropdown.value = 0;
            }
            
            dropdown.onValueChanged.AddListener(SaveChanges);
        }
    }

    private void SaveChanges(int option)
    {
        var selectedPawns = new List<PawnData>();
        
        foreach (var dropdown in Dropdowns)
        {
            var pawn = ((PawnOptionData) dropdown.options[dropdown.value]).Pawn;
            if(pawn == null) continue;
            
            selectedPawns.Add(pawn);
        }

        Application.Instance.PartyManager.SetSelectedParty(selectedPawns);
    }

    private void SaveChangesToPlayer(int option)
    {
        var pawn = ((PawnOptionData) PlayerDropdown.options[PlayerDropdown.value]).Pawn;
        
        if(pawn == null)
            return;
        
        Application.Instance.PlayerManager.SetNewPlayerPawn(pawn);
    }

    private void EndBonfireScene()
    {
        Application.Instance.SceneManager.EndBonfireScene();
    }
}

public class PawnOptionData : TMP_Dropdown.OptionData
{
    public PawnData Pawn;

    public PawnOptionData(PawnData pawn) : base(pawn?.Id ?? string.Empty)
    {
        Pawn = pawn;
    }
}
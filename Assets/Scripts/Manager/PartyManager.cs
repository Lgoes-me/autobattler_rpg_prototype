using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnController> AvailableParty { get; set; }
    private List<PawnController> SelectedParty { get; set; }

    public List<PawnController> GetSelectedParty()
    {
        if (SelectedParty == null)
        {
            SelectedParty = new List<PawnController>();
            
            foreach (var pawnName in Application.Instance.Save.SelectedParty)
            {
                var pawn = AvailableParty.FirstOrDefault(p => p.PawnData.name == pawnName);
                
                if(pawn == null)
                    continue;
                
                SelectedParty.Add(pawn);
            }
        }

        return SelectedParty;
    }
    
    public void SetSelectedParty(List<PawnController> selectedParty)
    {
        SelectedParty = new List<PawnController>(selectedParty);
        Application.Instance.Save.SelectedParty = SelectedParty.Select(p => p.PawnData.name).ToList();
        Application.Instance.SaveManager.SaveData(Application.Instance.Save);
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnController> AvailableParty { get; set; }
    private List<PawnController> SelectedParty { get; set; }
    
    public void Start()
    {
        SelectedParty = new List<PawnController>();
            
        foreach (var pawnName in Application.Instance.Save.SelectedParty)
        {
            var pawn = AvailableParty.FirstOrDefault(p => p.PawnData.name == pawnName);
                
            if(pawn == null)
                continue;

            //var playerPosition = Application.Instance.PlayerManager.PawnController.transform.position;
            //pawn = Instantiate(pawn, playerPosition, Quaternion.identity, transform);
            
            SelectedParty.Add(pawn);
        }
    }
    public List<PawnController> GetSelectedParty()
    {
        return SelectedParty;
    }
    
    public void SetSelectedParty(List<PawnController> selectedParty)
    {
        SelectedParty = new List<PawnController>(selectedParty);
        Application.Instance.Save.SelectedParty = SelectedParty.Select(p => p.PawnData.name).ToList();
        Application.Instance.SaveManager.SaveData(Application.Instance.Save);
    }
}
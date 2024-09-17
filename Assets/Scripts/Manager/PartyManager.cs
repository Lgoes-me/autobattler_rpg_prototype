using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnController> AvailableParty { get; set; }
    public List<PawnController> SelectedParty { get; private set; }
    
    private void Start()
    {
        SelectedParty = new List<PawnController>();
        var selectedParty = new List<PawnController>();
            
        foreach (var pawnName in Application.Instance.Save.SelectedParty)
        {
            var pawn = AvailableParty.FirstOrDefault(p => p.PawnData.name == pawnName);
                
            if(pawn == null)
                continue;
            
            selectedParty.Add(pawn);
        }
    
        SpawnSelectedPawns(selectedParty);
    }

    private void SpawnSelectedPawns(List<PawnController> pawns)
    {
        for (var index = SelectedParty.Count - 1; index >= 0 ; index--)
        {
            var pawn = SelectedParty[index];
            
            if(pawns.Contains(pawn))
                continue;
            
            Destroy(pawn.gameObject);
            SelectedParty.Remove(pawn);
        }
        
        foreach (var pawn in pawns)
        {
            if(pawn == null || SelectedParty.Contains(pawn))
                continue;

            var playerPosition = Application.Instance.PlayerManager.PawnController.transform.position;
            
            var randomRotation =  Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
            var pawnInstance = Instantiate(pawn, playerPosition + randomRotation, Quaternion.identity, transform);
            
            SelectedParty.Add(pawnInstance);
        }
    }
    
    public void SetSelectedParty(List<PawnController> selectedParty)
    {
        SpawnSelectedPawns(selectedParty);
        Application.Instance.Save.SelectedParty = SelectedParty.Select(p => p.PawnData.name).ToList();
        Application.Instance.SaveManager.SaveData(Application.Instance.Save);
    }

    public void SetPartyToFollow(bool transportToPlayer)
    {
        var party = Application.Instance.PartyManager.SelectedParty;
        var player = Application.Instance.PlayerManager.PawnController;

        for (var index = 0; index < party.Count; index++)
        {
            var pawn = party[index];
            pawn.PlayerFollowController.StopFollow();
            
            pawn.PlayerFollowController.StartFollow( 
                index == 0 ? player : party[index -1],
                transportToPlayer ? player.transform.position : Vector3.zero);
        }
    }
    
    public void StopPartyFollow()
    {
        var party = Application.Instance.PartyManager.SelectedParty;

        foreach (var pawn in party)
        {
            pawn.PlayerFollowController.StopFollow();
        }
    }
}
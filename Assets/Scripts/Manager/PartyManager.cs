using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnData> AvailableParty { get; set; }
    [field: SerializeField] public PawnController PawnController { get; set; }
    
    public List<PawnData> SelectedPawns { get; private set; }
    public List<PawnController> Party { get; private set; }
    
    public void Init()
    {
        SelectedPawns = new List<PawnData>();
        Party = new List<PawnController>();
            
        foreach (var pawnId in Application.Instance.Save.SelectedParty)
        {
            var pawn = AvailableParty.FirstOrDefault(p => p.Id == pawnId);
                
            if(pawn == null)
                continue;
            
            SelectedPawns.Add(pawn);
        }
    
        SpawnSelectedPawns();
    }

    private void SpawnSelectedPawns()
    {
        foreach (var pawn in Party)
        {
            Destroy(pawn.gameObject);
        }
        
        Party.Clear();

        foreach (var pawnData in SelectedPawns)
        {
            var playerPosition = Application.Instance.PlayerManager.PawnController.transform.position;
            
            var randomRotation =  Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
            var pawnInstance = Instantiate(PawnController, playerPosition + randomRotation, Quaternion.identity, transform);
            pawnInstance.SetCharacter(pawnData);
            Party.Add(pawnInstance);
        }
    }
    
    public void SetSelectedParty(List<PawnData> newSelectedParty)
    {
        Application.Instance.Save.SelectedParty = newSelectedParty.Select(p => p.Id).ToList();
        Application.Instance.SaveManager.SaveData(Application.Instance.Save);
        
        foreach (var pawnId in Application.Instance.Save.SelectedParty)
        {
            var pawn = AvailableParty.FirstOrDefault(p => p.Id == pawnId);
                
            if(pawn == null)
                continue;
            
            SelectedPawns.Add(pawn);
        }
        
        SpawnSelectedPawns();
    }

    public void SetPartyToFollow(bool transportToPlayer)
    {
        var player = Application.Instance.PlayerManager.PawnController;

        for (var index = 0; index < Party.Count; index++)
        {
            var pawn = Party[index];
            pawn.PlayerFollowController.StopFollow();
            
            pawn.PlayerFollowController.StartFollow( 
                index == 0 ? player : Party[index -1],
                transportToPlayer ? player.transform.position : Vector3.zero);
        }
    }
    
    public void StopPartyFollow()
    {
        foreach (var pawn in Party)
        {
            pawn.PlayerFollowController.StopFollow();
        }
    }
}
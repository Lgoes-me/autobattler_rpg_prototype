using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnData> AvailableParty { get; private set; }
    [field: SerializeField] private PawnController PawnController { get; set; }
    
    public List<PawnData> SelectedPawns { get; private set; }
    public List<PawnController> Party { get; private set; }
    public List<Archetype> Archetypes { get; private set; }
    
    private ArchetypeFactory ArchetypeFactory { get; set; }
    
    public void Init()
    {
        SelectedPawns = new List<PawnData>();
        Party = new List<PawnController>();
        
        Archetypes = new List<Archetype>();
        ArchetypeFactory = new ArchetypeFactory();
        
        SpawnSelectedPawns();
    }

    private void SpawnSelectedPawns()
    {
        SelectedPawns.Clear();

        foreach (var (pawnId, pawnInfo) in Application.Instance.Save.SelectedParty)
        {
            var pawn = AvailableParty.FirstOrDefault(p => p.Id == pawnId);
                
            if(pawn == null)
                continue;
            
            SelectedPawns.Add(pawn);
        }

        var positionsDict = new Dictionary<string, Vector3>();
        
        foreach (var pawnController in Party)
        {
            positionsDict.Add(pawnController.Pawn.Id, pawnController.transform.position);
            Destroy(pawnController.gameObject);
        }
        
        Party.Clear();

        foreach (var pawnData in SelectedPawns)
        {

            var playerPosition = Application.Instance.PlayerManager.PawnController.transform.position;
            var randomRotation =  Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;

            var pawnPosition = positionsDict.TryGetValue(pawnData.Id, out var position)
                ? position
                : playerPosition + randomRotation;
            
            var pawnInstance = Instantiate(PawnController, pawnPosition, Quaternion.identity, transform);
            pawnInstance.SetCharacter(pawnData);
            Party.Add(pawnInstance);
        }

        var archetypes = SelectedPawns
            .Select(p => p.Archetypes)
            .GroupBy(a => a)
            .Select(g =>  new { Key = g.Key.First(), Count = g.Count()})
            .ToList();
        
        foreach (var pair in archetypes)
        {
           Archetypes.Add(ArchetypeFactory.CreateArchetype(pair.Key, pair.Count));
        }
    }
    
    public void SetSelectedParty(List<PawnData> newSelectedParty)
    {
        Application.Instance.Save.SelectedParty = newSelectedParty.ToDictionary(p => p.Id, p => new PawnInfo(p.Id, 0));
        Application.Instance.SaveManager.SaveData(Application.Instance.Save);

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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnControllerPrefab { get; set; }

    public int PartySizeLimit { get; private set; }
    
    public List<BasePawn> AvailableParty { get; private set; }
    public List<PawnController> Party { get; private set; }

    private PlayerManager PlayerManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private ContentManager ContentManager { get; set; }
    private ArchetypeManager ArchetypeManager { get; set; }

    private void Awake()
    {
        PartySizeLimit = 8;
        Party = new List<PawnController>();
    }

    public void Prepare()
    {
        PlayerManager = Application.Instance.PlayerManager;
        GameSaveManager = Application.Instance.GameSaveManager;
        ContentManager = Application.Instance.ContentManager;
        ArchetypeManager = Application.Instance.ArchetypeManager;
    }

    public void GetAndSpawnAvailableParty()
    {
        AvailableParty = GameSaveManager.GetAvailableParty();
    }

    public void UnSpawnParty()
    {
        foreach (var pawn in Party)
        {
            Destroy(pawn.gameObject);
        }

        Party.Clear();
    }
    
    public void SpawnPartyAt(Vector3 position)
    {
        var playerPosition = position;
        
        foreach (var pawnInfo in GameSaveManager.GetSelectedParty())
        {
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
            var pawnInstance = Instantiate(PawnControllerPrefab, playerPosition + randomRotation, Quaternion.identity, transform);
            playerPosition = pawnInstance.transform.position;
            
            var pawn = ContentManager.GetPawnDomainFromInfo(pawnInfo);
            GameSaveManager.ApplyPawnInfo(pawn);
            pawnInstance.Init(pawn);
            Party.Add(pawnInstance);
        }

        PlayerManager.SetNewPlayerPawn(Party[0]);
        
        ArchetypeManager.CreateArchetypes(Party);
    }
    
    public void SetSelectedParty(List<BasePawn> newSelectedParty)
    {
        GameSaveManager.SetParty(newSelectedParty);
    }

    public void SetPartyToFollow(bool transportToPlayer)
    {
        var player = Party[0];

        for (var index = 1; index < Party.Count; index++)
        {
            var pawn = Party[index];
            var playerFollow = pawn.GetComponent<PlayerFollowController>();

            playerFollow.StopFollow();

            playerFollow.StartFollow(
                index == 0 ? player : Party[index - 1],
                transportToPlayer ? player.transform.position : Vector3.zero);
        }
    }

    public void StopPartyFollow()
    {
        for (var index = 1; index < Party.Count; index++)
        {
            var pawn = Party[index];
            var playerFollow = pawn.GetComponent<PlayerFollowController>();

            playerFollow.StopFollow();
        }
    }

    public void AddToAvailableParty(PawnData pawnData)
    {
        GameSaveManager.AddToAvailableParty(pawnData);
        AvailableParty = GameSaveManager.GetAvailableParty();
    }

    public bool Contains(PawnData pawn)
    {
        return Party.Any(p => p.Pawn.Id == pawn.Id);
    }
}
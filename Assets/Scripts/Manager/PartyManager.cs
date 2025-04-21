using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour, IManager
{
    [field: SerializeField] private PawnController PawnControllerPrefab { get; set; }
    [field: SerializeField] public PawnController BasePawnPrefab { get; set; }

    public int PartySizeLimit { get; private set; }
    
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
        PlayerManager = Application.Instance.GetManager<PlayerManager>();
        GameSaveManager = Application.Instance.GetManager<GameSaveManager>();
        ContentManager = Application.Instance.GetManager<ContentManager>();
        ArchetypeManager = Application.Instance.GetManager<ArchetypeManager>();
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
        var pawnPosition = position;
        
        foreach (var pawnInfo in GameSaveManager.GetSelectedParty())
        {
            var pawnInstance = AddToCurrentParty(pawnPosition, pawnInfo);
            pawnPosition = pawnInstance.transform.position;
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
    }

    public bool Contains(PawnData pawn)
    {
        return Party.Any(p => p.Pawn.Id == pawn.Id);
    }

    public void UpdatePawn(PawnInfo pawnInfo)
    {
        Party.FirstOrDefault(p => p.Pawn.Id == pawnInfo.Name)?.UpdatePawn(pawnInfo);
    }

    public PawnController AddToCurrentParty(Vector3 position, PawnInfo pawnInfo)
    {
        var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
        var pawnInstance = Instantiate(PawnControllerPrefab, position + randomRotation, Quaternion.identity, transform);

        var pawn = ContentManager.GetPawnDomainFromInfo(pawnInfo);
        GameSaveManager.ApplyPawnInfo(pawn);
        pawnInstance.Init(pawn);
        Party.Add(pawnInstance);
        
        ArchetypeManager.CreateArchetypes(Party);

        return pawnInstance;
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour, IManager
{
    [field: SerializeField] private PawnController PawnControllerPrefab { get; set; }
    [field: SerializeField] public PawnController BasePawnPrefab { get; private set; }

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

        var list = GameSaveManager.GetSelectedParty();
        
        for (var index = 0; index < list.Count; index++)
        {
            var pawnInfo = list[index];
            var pawnInstance = AddToCurrentParty(pawnPosition, pawnInfo, index != 0);
            pawnPosition = pawnInstance.transform.position;
        }

        PlayerManager.SetNewPlayerPawn(Party[0]);
        ArchetypeManager.CreateArchetypes(Party);
    }
    
    public void SetSelectedParty(List<Pawn> newSelectedParty)
    {
        GameSaveManager.SetParty(newSelectedParty);
        GameSaveManager.SaveCurrentGameState();
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
        GameSaveManager.SaveCurrentGameState();
    }

    public bool Contains(PawnData pawn)
    {
        return Party.Any(p => p.Pawn.Id == pawn.Id);
    }

    public void UpdatePawn(PawnInfo pawnInfo)
    {
        Party.FirstOrDefault(p => p.Pawn.Id == pawnInfo.Name)?.UpdatePawn(pawnInfo);
        GameSaveManager.SaveCurrentGameState();
    }

    public PawnController AddToCurrentParty(Vector3 position, PawnInfo pawnInfo, bool randomEnabled)
    {
        var randomRotation = randomEnabled ? Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f : Vector3.zero;
        var pawnInstance = Instantiate(PawnControllerPrefab, position + randomRotation, Quaternion.identity, transform);

        var pawn = ContentManager.GetPawnFromInfo(pawnInfo);
        var savedPawnInfo = GameSaveManager.GetPawnInfo(pawn);
        
        pawn.SetPawnInfo(savedPawnInfo);

        pawnInstance.Init(pawn);
        Party.Add(pawnInstance);
        
        ArchetypeManager.CreateArchetypes(Party);

        return pawnInstance;
    }
}
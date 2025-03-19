using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
        SpawnSelectedPawns();
    }

    private void SpawnSelectedPawns()
    {
        for (var index = 1; index < Party.Count; index++)
        {
            var pawnController = Party[index];
            Destroy(pawnController.gameObject);
        }

        Party.Clear();

        var selectedPawns = new List<BasePawn>();

        foreach (var pawnInfo in GameSaveManager.GetSelectedParty())
        {
            var pawn = AvailableParty.FirstOrDefault(p => p.Id == pawnInfo.Name);

            if (pawn == null)
                continue;

            selectedPawns.Add(pawn);
        }
        
        var playerPawn = ContentManager.GetPawnDomainFromBase(selectedPawns[0]);
        PlayerManager.SetNewPlayerPawn(playerPawn);
        Party.Add(PlayerManager.PawnController);

        for (var index = 1; index < selectedPawns.Count; index++)
        {
            var playerPosition = Party[0].transform.position;
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
            var pawnInstance = Instantiate(PawnControllerPrefab, playerPosition + randomRotation, Quaternion.identity,
                transform);

            var pawn = ContentManager.GetPawnDomainFromBase(selectedPawns[index]);
            GameSaveManager.ApplyPawnInfo(pawn);

            pawnInstance.Init(pawn);
            Party.Add(pawnInstance);
        }

        ArchetypeManager.CreateArchetypes(Party);
    }

    public void SetSelectedParty(List<BasePawn> newSelectedParty)
    {
        GameSaveManager.SetParty(newSelectedParty);
        SpawnSelectedPawns();
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnData> AvailableParty { get; private set; }
    [field: SerializeField] private PawnController PawnControllerPrefab { get; set; }

    [field: SerializeField] private PlayerManager PlayerManager { get; set; }
    [field: SerializeField] private GameSaveManager GameSaveManager { get; set; }
    [field: SerializeField] private InterfaceManager InterfaceManager { get; set; }
    
    public List<PawnController> Party { get; private set; }
    public List<Archetype> Archetypes { get; private set; }

    private ArchetypeFactory ArchetypeFactory { get; set; }

    public void Init()
    {
        Party = new List<PawnController>();

        Archetypes = new List<Archetype>();
        ArchetypeFactory = new ArchetypeFactory();

        SpawnSelectedPawns();
    }

    private void SpawnSelectedPawns()
    {
        var selectedPawns = new List<PawnData>();

        foreach (var (pawnId, _) in GameSaveManager.GetSelectedParty())
        {
            var pawn = AvailableParty.FirstOrDefault(p => p.Id == pawnId);

            if (pawn == null)
                continue;

            selectedPawns.Add(pawn);
        }

        var positionsDict = new Dictionary<string, Vector3>();

        foreach (var pawnController in Party)
        {
            positionsDict.Add(pawnController.Pawn.Id, pawnController.transform.position);
            Destroy(pawnController.gameObject);
        }

        Party.Clear();

        foreach (var pawnData in selectedPawns)
        {
            var playerPosition = PlayerManager.PawnController.transform.position;
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;

            var pawnPosition = positionsDict.TryGetValue(pawnData.Id, out var position)
                ? position
                : playerPosition + randomRotation;

            var pawnInstance = Instantiate(PawnControllerPrefab, pawnPosition, Quaternion.identity, transform);
            pawnInstance.SetCharacter(pawnData);
            Party.Add(pawnInstance);
        }

        Archetypes.Clear();

        var pawns = new List<PawnController>();
        pawns.Add(PlayerManager.PawnController);
        pawns.AddRange(Party);

        var archetypes = pawns
            .Select(p => p.Pawn.Archetypes)
            .SelectMany(a => a)
            .GroupBy(a => a)
            .Select(g => new {g.Key, Count = g.Count()})
            .ToList();

        foreach (var pair in archetypes)
        {
            Archetypes.Add(ArchetypeFactory.CreateArchetype(pair.Key, pair.Count));
        }

        var playerPawns = new List<PawnController>();

        playerPawns.Add(PlayerManager.PawnController);
        playerPawns.AddRange(Party);

        InterfaceManager.InitProfileCanvas(playerPawns);
        InterfaceManager.InitArchetypesCanvas(Archetypes);
    }

    public void SetSelectedParty(List<PawnData> newSelectedParty)
    {
        GameSaveManager.SetParty(newSelectedParty);
        SpawnSelectedPawns();
    }

    public void SetPartyToFollow(bool transportToPlayer)
    {
        var player = PlayerManager.PawnController;

        for (var index = 0; index < Party.Count; index++)
        {
            var pawn = Party[index];
            pawn.PlayerFollowController.StopFollow();

            pawn.PlayerFollowController.StartFollow(
                index == 0 ? player : Party[index - 1],
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
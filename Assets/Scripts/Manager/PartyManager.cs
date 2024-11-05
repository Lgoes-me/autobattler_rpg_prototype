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

    public int PartySizeLimit { get; private set; }
    public List<PawnController> Party { get; private set; }
    public List<Archetype> Archetypes { get; private set; }

    private ArchetypeFactory ArchetypeFactory { get; set; }

    public void Init()
    {
        PartySizeLimit = 5;
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

        for (var index = 1; index < Party.Count; index++)
        {
            var pawnController = Party[index];
            Destroy(pawnController.gameObject);
        }

        Party.Clear();

        PlayerManager.SetNewPlayerPawn(selectedPawns[0]);
        Party.Add(PlayerManager.PawnController);

        for (var index = 1; index < selectedPawns.Count; index++)
        {
            var pawnData = selectedPawns[index];

            var playerPosition = Party[0].transform.position;
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
            var pawnInstance = Instantiate(PawnControllerPrefab, playerPosition + randomRotation, Quaternion.identity,
                transform);

            var pawn = pawnData.ToDomain();
            GameSaveManager.ApplyPawnInfo(pawn);

            pawnInstance.Init(pawn);
            Party.Add(pawnInstance);
        }

        Archetypes.Clear();

        var archetypes = Party
            .Select(p => p.Pawn.Archetypes)
            .SelectMany(a => a)
            .GroupBy(a => a)
            .Select(g => new {g.Key, Count = g.Count()})
            .ToList();

        foreach (var pair in archetypes)
        {
            Archetypes.Add(ArchetypeFactory.CreateArchetype(pair.Key, pair.Count));
        }

        InterfaceManager.InitProfileCanvas(Party);
        InterfaceManager.InitArchetypesCanvas(Archetypes);
    }

    public void SetSelectedParty(List<PawnData> newSelectedParty)
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
            pawn.PlayerFollowController.StopFollow();

            pawn.PlayerFollowController.StartFollow(
                index == 0 ? player : Party[index - 1],
                transportToPlayer ? player.transform.position : Vector3.zero);
        }
    }

    public void StopPartyFollow()
    {
        for (var index = 1; index < Party.Count; index++)
        {
            Party[index].PlayerFollowController.StopFollow();
        }
    }
}
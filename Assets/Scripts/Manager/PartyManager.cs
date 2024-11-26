using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PartyManager : MonoBehaviour
{
    [field: SerializeField] private PawnController PawnControllerPrefab { get; set; }

    public List<PawnFacade> AvailableParty { get; private set; }
    public int PartySizeLimit { get; private set; }
    public List<PawnController> Party { get; private set; }
    public List<Archetype> Archetypes { get; private set; }

    private ArchetypeFactory ArchetypeFactory { get; set; }

    private PlayerManager PlayerManager { get; set; }
    private GameSaveManager GameSaveManager { get; set; }
    private InterfaceManager InterfaceManager { get; set; }
    private ContentManager ContentManager { get; set; }

    private void Awake()
    {
        PartySizeLimit = 8;
        Party = new List<PawnController>();

        Archetypes = new List<Archetype>();
        ArchetypeFactory = new ArchetypeFactory();
    }

    public void Prepare()
    {
        PlayerManager = Application.Instance.PlayerManager;
        GameSaveManager = Application.Instance.GameSaveManager;
        InterfaceManager = Application.Instance.InterfaceManager;
        ContentManager = Application.Instance.ContentManager;
    }

    public void GetAndSpawnAvailableParty()
    {
        AvailableParty = GameSaveManager.GetAvailableParty();
        SpawnSelectedPawns();
    }

    private void SpawnSelectedPawns()
    {
        var selectedPawns = new List<PawnFacade>();

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

        var playerPawn = ContentManager.GetPawnDomainFromFacade(selectedPawns[0]);
        PlayerManager.SetNewPlayerPawn(playerPawn);
        Party.Add(PlayerManager.PawnController);

        for (var index = 1; index < selectedPawns.Count; index++)
        {
            var playerPosition = Party[0].transform.position;
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
            var pawnInstance = Instantiate(PawnControllerPrefab, playerPosition + randomRotation, Quaternion.identity,
                transform);

            var pawn = ContentManager.GetPawnDomainFromFacade(selectedPawns[index]);
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

    public void SetSelectedParty(List<PawnFacade> newSelectedParty)
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
}
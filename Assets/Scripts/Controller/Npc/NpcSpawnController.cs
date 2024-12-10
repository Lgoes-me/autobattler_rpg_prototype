using UnityEngine;

public class NpcSpawnController : MonoBehaviour
{
    [field: SerializeField] private NpcSchedule NpcSchedule { get; set; }
    [field: SerializeField] private NpcController NpcControllerPrefab { get; set; }

    private TimeManager TimeManager { get; set; }
    private int CurrentState { get; set; }
    private NpcController NpcController { get; set; }

    private void Awake()
    {
        TimeManager = Application.Instance.TimeManager;
        CheckInitialSate();
    }

    private void CheckInitialSate()
    {
        CurrentState = -1;

        for (var index = 0; index < NpcSchedule.RoutinePlacement.Count; index++)
        {
            var npcPlacement = NpcSchedule.RoutinePlacement[index];

            if (index == NpcSchedule.RoutinePlacement.Count - 1)
                return;

            var nextPlacement = NpcSchedule.RoutinePlacement[index + 1];

            if (nextPlacement.Time > TimeManager.HorarioEmJogo && npcPlacement.Time < TimeManager.HorarioEmJogo)
            {
                CurrentState = index;
                SpawnPlacementState(NpcSchedule.RoutinePlacement[CurrentState]);
                return;
            }
        }
    }

    private void SpawnPlacementState(NpcPlacement placementData)
    {
        if(Application.Instance.PartyManager.Contains(NpcSchedule.PawnData))
            return;

        if (NpcController == null)
        {
            NpcController = Instantiate(NpcControllerPrefab, transform.position, Quaternion.identity)
                .Init(NpcSchedule.PawnData);
        }

        placementData.WithDialogue(NpcController).SpawnCharacterAt(NpcController);
    }

    private void FixedUpdate()
    {
        if (NpcSchedule.RoutinePlacement.Count <= CurrentState + 1)
        {
            CurrentState = -1;
            return;
        }

        if (Mathf.Abs(TimeManager.HorarioEmJogo - NpcSchedule.RoutinePlacement[CurrentState + 1].Time) < 0.1f)
        {
            CurrentState += 1;
            ActivatePlacementState(NpcSchedule.RoutinePlacement[CurrentState]);
        }
    }

    private void ActivatePlacementState(NpcPlacement placementData)
    {
        if(Application.Instance.PartyManager.Contains(NpcSchedule.PawnData))
            return;
        
        if (NpcController == null)
        {
            NpcController = Instantiate(NpcControllerPrefab, transform.position, Quaternion.identity)
                .Init(NpcSchedule.PawnData);
        }

        placementData.WithDialogue(NpcController).ControlCharacterController(NpcController);
    }
}
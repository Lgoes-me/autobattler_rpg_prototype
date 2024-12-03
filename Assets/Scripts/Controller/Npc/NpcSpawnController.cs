using System;
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
        CurrentState = -1;
    }

    private void FixedUpdate()
    {
        if (NpcSchedule.Routine.Count <= CurrentState + 1)
        {
            CurrentState = -1;
            return;
        }

        if (Mathf.Abs(TimeManager.HorarioEmJogo - NpcSchedule.Routine[CurrentState + 1].Time) < 0.1f)
        {
            CurrentState += 1;
            ActivatePlacementState(NpcSchedule.Routine[CurrentState]);
        }
    }

    private void ActivatePlacementState(NpcPlacementData placementData)
    {
        if (NpcController == null)
        {
            NpcController = Instantiate(NpcControllerPrefab, placementData.Placement.position, Quaternion.identity)
                .Init(NpcSchedule.PawnData);
        }

        switch (placementData.ScheduleEventType)
        {
            case ScheduleEventType.Spawn:
            {
                NpcController.WithDialogue(placementData.DialogueData);
                break;
            }
            case ScheduleEventType.WaitAt:
            {
                NpcController.WithPath(placementData.Placement).WithDialogue(placementData.DialogueData);
                break;
            }
            case ScheduleEventType.Despawn:
            {
                NpcController.WithPath(placementData.Placement).WithDialogue(placementData.DialogueData);
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
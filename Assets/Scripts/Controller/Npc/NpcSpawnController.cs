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
        if (NpcController == null)
        {
            NpcController = Instantiate(NpcControllerPrefab, transform.position, Quaternion.identity).Init(NpcSchedule.PawnData);
        }
        
        placementData.ControlCharacterController(NpcController);
    }
}
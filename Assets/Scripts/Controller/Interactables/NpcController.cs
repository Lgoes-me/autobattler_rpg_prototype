using UnityEngine;

public class NpcController : InteractableController
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private PawnController PawnController { get; set; }

    private void Start()
    {
        PawnController.Init(PawnData.ToDomain());
    }
}
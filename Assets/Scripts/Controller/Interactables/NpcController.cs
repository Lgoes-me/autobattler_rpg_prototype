using System;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : InteractableController
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private DialogueData Dialogue { get; set; }
    [field: SerializeField] private Action PathCallback { get; set; }

    public NpcController Init(PawnData pawnData)
    {
        PawnData = pawnData;

        var pawn = PawnData.ToDomain(TeamType.Player);
        PawnController.Init(pawn);

        return this;
    }

    public NpcController WithDialogue(DialogueData dialogue)
    {
        Dialogue = dialogue;
        return this;
    }

    public NpcController WithPath(Transform destination, Action callback)
    {
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = false;
        NavMeshAgent.destination = destination.position;
        PathCallback = callback;

        return this;
    }

    private void Update()
    {
        if (PathCallback != null &&
            NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
            NavMeshAgent.remainingDistance < 1f)
        {
            PathCallback();
            PathCallback = null;
        }
    }

    protected override void InternalSelect()
    {
        Application.Instance.PauseManager.PauseGame();
        
        Application.Instance.DialogueManager.OpenDialogue(Dialogue, () =>
        {
            Application.Instance.PauseManager.ResumeGame();
            Preselect();
        });
    }

    protected override void InternalUnSelect()
    {
        Application.Instance.DialogueManager.CloseDialogue();
    }

    public void DeSpawn()
    {
        Destroy(this.gameObject);
    }
}
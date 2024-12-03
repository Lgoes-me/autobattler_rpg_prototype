using UnityEngine;
using UnityEngine.AI;

public class NpcController : InteractableController
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private DialogueData Dialogue { get; set; }

    public NpcController Init(PawnData pawnData)
    {
        PawnData = pawnData;
        return this;
    }

    public NpcController WithDialogue(DialogueData dialogue)
    {
        Dialogue = dialogue;
        return this;
    }
    
    public NpcController WithPath(Transform destination)
    {
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = false;
        NavMeshAgent.destination = destination.position;
        
        return this;
    }

    
    private void Start()
    {
        var pawn = PawnData.ToDomain(TeamType.Player);
        PawnController.Init(pawn);
    }
    
    protected override void InternalSelect()
    {
        Application.Instance.DialogueManager.OpenDialogue(Dialogue, Preselect);
    }
    
    protected override void InternalUnSelect()
    {
        Application.Instance.DialogueManager.CloseDialogue();
    }
}
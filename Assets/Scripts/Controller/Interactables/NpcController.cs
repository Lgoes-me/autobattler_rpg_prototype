using System;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour, IInteractableListener
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private DialogueData Dialogue { get; set; }
    [field: SerializeField] private Action PathCallback { get; set; }

    [field: SerializeField] private InteractableController Controller { get; set; }

    private void Awake()
    {
        Controller.Interactable = this;
    }
    
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

    public void Select(Action callback)
    {
        Application.Instance.PauseManager.PauseGame();
        
        Application.Instance.DialogueManager.OpenDialogue(Dialogue, () =>
        {
            Application.Instance.PauseManager.ResumeGame();
            callback();
        });
    }

    public void UnSelect()
    {
        Application.Instance.DialogueManager.CloseDialogue();
    }

    public void DeSpawn()
    {
        Destroy(gameObject);
    }
}
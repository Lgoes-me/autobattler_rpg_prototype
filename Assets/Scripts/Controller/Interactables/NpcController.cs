using System;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour, IInteractableListener
{
    [field: SerializeField] private PawnData PawnData { get; set; }
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

        return this;
    }

    public NpcController WithDialogue(DialogueData dialogue)
    {
        Dialogue = dialogue;
        return this;
    }

    public NpcController WithPath(Transform destination, Action callback)
    {
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
        Application.Instance.GetManager<DialogueManager>().OpenDialogue(Dialogue, callback);
    }

    public void UnSelect()
    {
        Application.Instance.GetManager<DialogueManager>().CloseDialogue();
    }

    public void DeSpawn()
    {
        Destroy(gameObject);
    }
}
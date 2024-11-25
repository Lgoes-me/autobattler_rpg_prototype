using UnityEngine;

public class NpcController : InteractableController
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private DialogueData Dialogue { get; set; }

    private void Start()
    {
        PawnController.Init(PawnData.ToDomain(TeamType.Player));
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
using UnityEngine;

public class NpcController : InteractableController
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private PawnController PawnController { get; set; }
    [field: SerializeField] private DialogueData Dialogue { get; set; }

    public NpcController Init(PawnData pawnData)
    {
        PawnData = pawnData;
        return this;
    }

    public void SetDialogue(DialogueData dialogue)
    {
        Dialogue = dialogue;
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
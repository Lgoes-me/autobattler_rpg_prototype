using System.Collections;

public abstract class DialogueEvent : IDialogue
{
    public abstract IEnumerator ReadDialogue(DialogueManager dialogueManager, PawnData pawn);
}
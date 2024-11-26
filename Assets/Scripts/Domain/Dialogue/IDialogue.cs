using System.Collections;

public interface IDialogue : IComponentData
{
    public IEnumerator ReadDialogue(DialogueManager dialogueManager, BasePawn pawn);
}
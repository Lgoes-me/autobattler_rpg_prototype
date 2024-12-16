using System;

public interface IInteractableListener
{
    void Select(Action callback);
    void UnSelect();
}

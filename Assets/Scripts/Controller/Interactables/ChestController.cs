using System;
using UnityEngine;

public class ChestController : MonoBehaviour, IInteractableListener
{
    [field: SerializeReference] [field: SerializeField] public GameAction OpenChestAction { get; private set; }
    [field: SerializeField] private InteractableController Controller { get; set; }

    public void Start()
    {
        Controller.Interactable = this;
    }

    public void Select(Action callback)
    {
        OpenChestAction?.Invoke();
        callback?.Invoke();
    }

    public void UnSelect()
    {
        
    }
}
using System;
using UnityEngine;

public class BonfireController : MonoBehaviour, IInteractableListener
{
    [field: SerializeField] private SpawnController Spawn { get; set; }
    private bool Selected { get; set; }

    [field: SerializeField] private InteractableController Controller { get; set; }

    private void Awake()
    {
        Controller.Interactable = this;
    }

    public void Select(Action callback)
    {
        Selected = true;
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Application.Instance.SceneManager.StartBonfireScene(new SpawnDomain(Spawn.Id, scene), callback);
    }

    public void UnSelect()
    {
        Application.Instance.SceneManager.EndBonfireScene();
        Selected = false;
    }
}
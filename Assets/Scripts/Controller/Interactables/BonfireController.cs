using System;
using UnityEngine;

public class BonfireController : MonoBehaviour, IInteractableListener
{
    [field: SerializeField] public SpawnController Spawn { get; set; }
    private bool Selected { get; set; }

    [field: SerializeField] private InteractableController Controller { get; set; }

    private string SceneId { get; set; }

    public void Init(string sceneId)
    {
        Controller.Interactable = this;
        SceneId = sceneId;
    }

    public void Select(Action callback)
    {
        Selected = true;
        Application.Instance.GetManager<SceneManager>().StartBonfireScene(new SpawnDomain(Spawn.Id, SceneId), callback);
    }

    public void UnSelect()
    {
        Application.Instance.GetManager<SceneManager>().EndBonfireScene();
        Selected = false;
    }
}
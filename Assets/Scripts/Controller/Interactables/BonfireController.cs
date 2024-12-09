using Controller.Doors;
using UnityEngine;

public class BonfireController : InteractableController
{
    [field: SerializeField] private SpawnController Spawn { get; set; }
    private bool Selected { get; set; }

    protected override void InternalSelect()
    {
        Selected = true;
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Application.Instance.SceneManager.StartBonfireScene(this, new SpawnDomain(Spawn.Id, scene));
    }

    protected override void InternalUnSelect()
    {
        Application.Instance.SceneManager.EndBonfireScene();
        Selected = false;
    }
}
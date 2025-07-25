using Cinemachine;
using UnityEngine;

public class CorridorAreaController : SpawnController
{
    [field: SerializeField] private Transform Destination { get; set; }
    [field: SerializeField] private GameObject Spotlight { get; set; }
    [field: SerializeField] private SpriteRenderer FogDoor { get; set; }
    [field: SerializeField] private Color ActiveColor { get; set; }
    [field: SerializeField] private Color InactiveColor { get; set; }

    private Transition Transition { get; set; }
    private bool CanUse { get; set; } = true;

    public void Init(Transition transition)
    {
        Transition = transition;

        if (Application.Instance.GetManager<SceneManager>().IsOpen(Transition))
        {
            Spotlight.SetActive(true);
            FogDoor.color = ActiveColor;
        }
        else
        {
            Spotlight.SetActive(false);
            FogDoor.color = InactiveColor;
        }
    }
    
    public override async void SpawnPlayer(CinemachineBlendDefinition blend)
    {
        base.SpawnPlayer(blend);
        CanUse = false;

        var playerManager = Application.Instance.GetManager<PlayerManager>();
        
        await playerManager.MovePlayerTo(Destination);
        playerManager.EnablePlayerInput();
        CanUse = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanUse && other.CompareTag("Player"))
        {
            if (Application.Instance.GetManager<SceneManager>().IsOpen(Transition))
            {
                UseCorridor();
            }
            else
            {
                CanUse = false;
                
                var dialogue = Application.Instance.GetManager<SceneManager>().GetDialogue(Transition);

                if (dialogue != null)
                {
                    Application.Instance.GetManager<DialogueManager>().OpenDialogue(dialogue, () => CanUse = true);
                }
            }
        }
    }

    private void UseCorridor()
    {
        var playerManager = Application.Instance.GetManager<PlayerManager>();
        playerManager.DisablePlayerInput();
        Application.Instance.GetManager<SceneManager>().ChangeContext(Transition.Destination);
    }

    public DoorData ToDoorData()
    {
        return new DoorData
        {
            Name = gameObject.name,
            Id = Id
        };
    }
}
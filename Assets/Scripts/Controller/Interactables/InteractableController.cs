using UnityEngine;

public class InteractableController : MonoBehaviour , IPauseListener
{
    [field: SerializeField] private InteractableCanvasController InteractableCanvas { get; set; }
    [field: SerializeField] private InteractableStrategy Interactable { get; set; }
    
    private bool Enabled { get; set; }

    private void Awake()
    {
        Enabled = true;
        InteractableCanvas.Init(this);
        Application.Instance.PauseManager.SubscribePauseListener(this);
    }

    public void Preselect()
    {
        if(!Enabled)
            return;
        
        InteractableCanvas.Show();
        Interactable.PreSelect();
    }

    public void Select()
    {
        if(!Enabled)
            return;
        
        Interactable.Interact();
    }

    public void Unselect()
    {
        if(!Enabled)
            return;
        
        InteractableCanvas.Hide();
        Interactable.UnSelect();
    }

    public void Pause()
    {
        Enabled = false;
    }

    public void Resume()
    {
        Enabled = true;
    }

    private void OnDestroy()
    {
        Application.Instance.PauseManager.UnsubscribePauseListener(this);
    }
}
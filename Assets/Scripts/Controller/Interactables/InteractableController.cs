using UnityEngine;

public class InteractableController : MonoBehaviour , IPauseListener
{
    [field: SerializeField] private InteractableCanvasController InteractableCanvas { get; set; }
    
    public IInteractableListener Interactable { get; set; }
    public bool Enabled { get; set; }
    
    private bool Preselected { get; set; }

    private void Awake()
    {
        Enabled = true;
        InteractableCanvas.Init(this);
        Application.Instance.GetManager<PauseManager>().SubscribePauseListener(this);
    }

    public void Preselect()
    {
        if (!Enabled || Preselected)
            return;

        Preselected = true;
        InteractableCanvas.Show();
    }

    public void Select()
    {
        if (!Enabled)
            return;
        
        InteractableCanvas.Hide();
        
        Interactable.Select(() =>
        {
            Preselected = false;
            Preselect();
        });
    }
    
    public void Unselect()
    {
        if (!Enabled)
            return;
        
        Preselected = false;
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
        Application.Instance.GetManager<PauseManager>().UnsubscribePauseListener(this);
    }
}
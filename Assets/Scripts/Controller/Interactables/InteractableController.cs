using UnityEngine;

public abstract class InteractableController : MonoBehaviour , IPauseListener
{
    [field: SerializeField] private InteractableCanvasController InteractableCanvas { get; set; }
    
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
        InternalPreselect();
    }

    protected virtual void InternalPreselect()
    {
        
    }

    public void Select()
    {
        if(!Enabled)
            return;
        
        InternalSelect();
        InteractableCanvas.Hide();
    }

    protected virtual void InternalSelect()
    {
        
    }
    
    public void Unselect()
    {
        if(!Enabled)
            return;
        
        InteractableCanvas.Hide();
        InternalUnSelect();
    }

    protected virtual void InternalUnSelect()
    {
        
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
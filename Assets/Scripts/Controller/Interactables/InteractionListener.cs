using UnityEngine;

public abstract class InteractionListener : MonoBehaviour
{
    public abstract void Interact();

    public virtual void PreSelect()
    {
        
    }
    
    public virtual void UnSelect()
    {
        
    }
}

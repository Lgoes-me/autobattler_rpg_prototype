using UnityEngine;

public abstract class InteractableStrategy : MonoBehaviour
{
    public abstract void Interact();

    public virtual void PreSelect()
    {
        
    }
    
    public virtual void UnSelect()
    {
        
    }
}

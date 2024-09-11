using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    
    public void DoAnimationEvent()
    {
        AnimationStateController.CurrentState.DoAnimationEvent();
    }
}
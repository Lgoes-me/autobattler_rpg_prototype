using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    
    public void DoAnimationEvent()
    {
        Debug.Log($"DoAnimationEvent {gameObject.name} {AnimationStateController.CurrentState.Animation}");
        AnimationStateController.CurrentState.DoAnimationEvent();
    }
}
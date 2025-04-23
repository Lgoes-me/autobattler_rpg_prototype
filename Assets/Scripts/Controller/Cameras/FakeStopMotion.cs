using UnityEngine;

public class FakeStopMotion : MonoBehaviour
{
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private  int FPS { get; set; }
    
    private float Time { get; set; }

    void FixedUpdate()
    {
        Time += UnityEngine.Time.fixedDeltaTime;
        var updateTime = 1f / FPS;
        Animator.speed = 0;

        if (Time > updateTime)
        {
            Time -= updateTime;
            Animator.speed = 60f / FPS;
        }
    }
}
//https://github.com/EricFreeman/FakeStopMotion/blob/master/Assets/FakeStopMotion.cs
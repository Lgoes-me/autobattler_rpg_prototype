using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [field: SerializeField] private float Speed { get; set; }

    private PawnController Creator { get; set; }
    private List<AbilityEffect> Effects { get; set; }
    private Vector3 Destination { get; set; }
    private AnimationCurve Trajectory { get; set; }

    private float LifeTime { get; set; }

    public void Init(
        PawnController creator,
        List<AbilityEffect> effects,
        Vector3 destination,
        AnimationCurve trajectory,
        int error)
    {
        Creator = creator;
        Effects = effects;
        var randomError = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * Random.Range(0, error);
        Destination = destination + randomError;
        Trajectory = trajectory;

        LifeTime = 0f;
    }

    private void FixedUpdate()
    {
        var position = transform.position;
        
        //Debuggar o porque 20/3 ?????
        
        
        position = Vector3.Lerp(position, Destination, LifeTime);
        position = new Vector3(position.x, Trajectory.Evaluate(LifeTime * 20 / 3), position.z);
        transform.rotation = Quaternion.LookRotation(Destination - position);
        
        transform.position = position;

        LifeTime += Time.deltaTime * 0.1f * Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PawnController>(out var pawnController) &&
            pawnController.Pawn.Team != Creator.Pawn.Team &&
            pawnController.PawnState.CanBeTargeted)
        {
            foreach (var effect in Effects)
            {
                effect.DoAbilityEffect(pawnController);
            }

            Destroy(gameObject);
        }
    }
}
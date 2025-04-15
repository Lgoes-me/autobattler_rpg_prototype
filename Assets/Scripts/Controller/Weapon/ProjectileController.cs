using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileController : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField] private float Speed { get; set; }

    private PawnController Creator { get; set; }
    private List<AbilityEffect> Effects { get; set; }
    private Vector3 Destination { get; set; }
    private AnimationCurve Trajectory { get; set; }

    private float LifeTime { get; set; }

    public ProjectileController Init(
        PawnController creator,
        List<AbilityEffect> effects,
        Vector3 destination,
        AnimationCurve trajectory,
        float error)
    {
        Creator = creator;
        Effects = effects;
        
        var randomError = 
            Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * 
            Vector3.forward *
            Random.Range(0, error);
        
        Destination = destination + randomError;
        Trajectory = trajectory;

        LifeTime = 0f;

        return this;
    }

    public void OverrideSprite(Sprite projectile)
    {
        SpriteRenderer.sprite = projectile;
    }

    private void FixedUpdate()
    {
        if(LifeTime > 0.3)
            return;
            
        var position = transform.position;

        position = Vector3.Lerp(position, Destination, LifeTime);
        position = new Vector3(position.x, Trajectory.Evaluate(LifeTime * 20 / 3), position.z);

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(Destination - position);

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
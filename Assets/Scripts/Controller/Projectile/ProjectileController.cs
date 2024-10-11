using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [field:SerializeField] private float Duration { get; set; }
    [field:SerializeField] private float Speed { get; set; }
    [field:SerializeField] private Rigidbody Rigidbody { get; set; }
    
    private PawnController Creator { get; set; }
    private List<AbilityEffect> Effects { get; set; }
    
    public void Init(PawnController creator, List<AbilityEffect> effects, Vector3 direction)
    {
        Creator = creator;
        Effects = effects;
        Rigidbody.velocity = direction * Speed;

        StartCoroutine(SelfDestructCoroutine());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PawnController>(out var pawnController) &&
            pawnController.Team != Creator.Team &&
            pawnController.PawnState.CanBeTargeted)
        {
            foreach (var effect in Effects)
            {
                effect.DoAbilityEffect(pawnController);
            }
            Destroy(this.gameObject);
        }
    }
    
    private IEnumerator SelfDestructCoroutine()
    {
        yield return new WaitForSeconds(Duration);

        if (!gameObject.activeInHierarchy)
            yield break;
        
        Destroy(this.gameObject);
    }
}

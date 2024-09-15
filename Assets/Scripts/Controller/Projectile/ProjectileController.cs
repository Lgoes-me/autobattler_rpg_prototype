using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [field:SerializeField] private float Duration { get; set; }
    [field:SerializeField] private float Speed { get; set; }
    [field:SerializeField] private Rigidbody Rigidbody { get; set; }
    
    private PawnController Creator { get; set; }
    private Attack Attack { get; set; }
    
    public void Init(PawnController creator, Attack attack, Vector3 direction)
    {
        Creator = creator;
        Attack = attack;
        Rigidbody.velocity = direction * Speed;

        StartCoroutine(SelfDestructCoroutine());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PawnController>(out var pawnController) &&
            pawnController.Team != Creator.Team &&
            pawnController.PawnState.CanBeTargeted)
        {
            Attack.DoAttackToPawn(pawnController);
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

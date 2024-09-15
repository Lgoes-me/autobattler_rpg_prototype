using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [field:SerializeField] private Rigidbody Rigidbody { get; set; }
    
    private PawnController Creator { get; set; }
    private Attack Attack { get; set; }
    
    public void Init(PawnController creator, Attack attack)
    {
        Creator = creator;
        Attack = attack;
        Rigidbody.velocity = transform.forward;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PawnController>(out var pawnController) && pawnController.Team != Creator.Team)
        {
            Attack.DoAttackToPawn(pawnController);
            Destroy(this.gameObject);
        }
    }
    
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}

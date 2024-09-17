﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] public PawnData PawnData { get; private set; }
    [field: SerializeField] public PlayerFollowController PlayerFollowController { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private PawnCanvasController PawnCanvasController { get; set; }
    
    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    [field: SerializeField] private CharacterController CharacterController { get; set; }
    [field: SerializeField] private Transform SpawnPoint { get; set; }
    
    [field: SerializeField] public TeamType Team { get; private set; }

    public PawnDomain Pawn { get; private set; }
    public AnimationState PawnState => AnimationStateController.CurrentState;
    private Attack Attack { get; set; }
    private Coroutine BackToIdleCoroutine { get; set; }
    private Attack SpecialAttackRequested { get; set; }

    public PawnController Init(PawnCanvasController pawnCanvasController = null)
    {
        Pawn = PawnData.ToDomain();
        enabled = true;
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        if (pawnCanvasController != null)
            PawnCanvasController = pawnCanvasController;

        PawnCanvasController.Init(this);

        Attack = null;

        return this;
    }

    public IEnumerator Turno(List<PawnController> pawns)
    {
        
        Attack = SpecialAttackRequested ?? Pawn.GetCurrentAttackIntent().ToDomain(this);
        Attack.ChooseFocus(pawns);

        var direction = Attack.Destination - transform.position;
        
        if (direction.magnitude > Attack.Range)
        {
            AnimationStateController.SetAnimationState(new IdleState());
        }
        else
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.SetDestination(transform.position);
            CharacterController.SetSpeed(0);
            CharacterController.SetDirection(direction);
            
            AnimationStateController.SetAnimationState(new AttackState(Attack, AttackEnemy), GoBackToIdle);
        }
        
        yield break;
    }

    private void AttackEnemy()
    {
        if(Attack == null || !PawnState.AbleToFight)
            return;

        if (Pawn.HasMana)
        {
            if (Attack.ManaCost > 0)
            {
                SpecialAttackRequested = null;
                Pawn.Mana = 0;
                PawnCanvasController.UpdateMana();
            }
            else 
            {
                Pawn.Mana = Mathf.Clamp(Pawn.Mana + 10, 0, Pawn.MaxMana);
                PawnCanvasController.UpdateMana();
            }
        }

        Application.Instance.AudioManager.PlaySound(SfxType.Slash);

        if (Attack.Projectile != null)
        {
            var direction = Attack.Destination - SpawnPoint.position;
            direction = new Vector3(direction.x, 0, direction.z);
            
            Instantiate(Attack.Projectile, SpawnPoint.position, Quaternion.LookRotation(direction)).Init(this, Attack, direction);
        }
        else
        {
            Attack.DoAttack();
        }
    }
    
    private void GoBackToIdle()
    {
        if(BackToIdleCoroutine != null)
            StopCoroutine(BackToIdleCoroutine);
        
        BackToIdleCoroutine = StartCoroutine(GoBackToIdleCoroutine());
    }
    
    private IEnumerator GoBackToIdleCoroutine()
    {
        yield return new WaitForSeconds(Attack.Delay);

        if (!PawnState.AbleToFight)
            yield break;
        
        AnimationStateController.SetAnimationState(new IdleState());
    }

    public void ReceiveAttack(Attack attack)
    {
        Pawn.Health = Mathf.Clamp(Pawn.Health - attack.Damage.Value, 0, Pawn.MaxHealth);
        var dead = Pawn.Health <= 0;
        CharacterController.DoHitStop();
        PawnCanvasController.UpdateLife(dead);

        if (!dead) return;
        
        AnimationStateController.SetAnimationState(new DeadState());
        NavMeshAgent.isStopped = true;
        Attack = null;
    }

    private void Update()
    {
        if (Attack == null || !PawnState.CanWalk)
            return;

        CharacterController.SetDirection(NavMeshAgent.velocity);
        var direction = Attack.Destination - transform.position;
        
        if (Attack != null && Attack.Range >= direction.magnitude)
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.SetDestination(transform.position);
            CharacterController.SetSpeed(0);
        }
        else if(NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && NavMeshAgent.remainingDistance < 1f)
        {
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * (Attack.Range - 1);
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Attack.Destination + randomRotation);
            CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
        }
    }

    public void Deactivate()
    {
        enabled = false;
    }

    public void DoSpecial(AttackData attackData)
    {
        SpecialAttackRequested = attackData.ToDomain(this);
    }
    
    public void Dance()
    {
        PawnCanvasController.Hide();
        AnimationStateController.SetAnimationState(new DanceState(), GoBackToIdle);
    }
    
    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;

        PawnController pawnController = obj as PawnController;
        
        if (pawnController == null)
            return false;

        return pawnController.PawnData.name == PawnData.name;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
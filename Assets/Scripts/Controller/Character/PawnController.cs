﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] private BasePawnCanvasController CanvasController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }

    private Coroutine BackToIdleCoroutine { get; set; }
    private Ability Ability { get; set; }
    
    public CharacterController CharacterController { get; private set; }
    public Pawn Pawn { get; private set; }
    public AnimationState PawnState => CharacterController.CurrentState;
    public BattleController BattleController { get; private set; }

    public void Init(Pawn pawn)
    {
        Pawn = pawn;
        CharacterController = Instantiate(pawn.Character, transform);
        CharacterController.SetWeapon(pawn.Weapon);
        
        if (CanvasController != null)
        {
            CanvasController.Init(this);
        }

        Pawn.LostLife += ReceiveAttack;
        Pawn.GainedLife += ReceiveHeal;
    }

    public void UpdatePawn(PawnInfo pawnInfo)
    {
        Pawn.SetPawnInfo(pawnInfo);
        CharacterController.SetWeapon(Pawn.Weapon);
    }

    public void RemoveCanvasController()
    {
        Destroy(CanvasController.gameObject);
        CanvasController = null;
    }

    public void StartBattle(BattleController battleController, Battle battle)
    {
        Pawn.StartBattle(battle);

        enabled = true;
        Ability = null;

        BattleController = battleController;
        CharacterController.SetAnimationState(new IdleState());
    }

    public void RealizeTurn()
    {
        if (PawnState is not IdleState)
            return;

        Ability = Pawn.GetCurrentAttackIntent(this, BattleController.Battle);
        Ability.ChooseFocus(this, BattleController.Battle);
    }

    private void DoAbility(Ability ability)
    {
        if (!PawnState.AbleToFight)
            return;

        if (ability.IsSpecial)
        {
            Application.Instance.GetManager<BattleEventsManager>().DoSpecialAttackEvent(BattleController.Battle, this, ability);
        }
        else
        {
            Application.Instance.GetManager<BattleEventsManager>().DoAttackEvent(BattleController.Battle, this, ability);
        }

        Application.Instance.GetManager<AudioManager>().PlaySound(SfxType.Slash);

        ability.SpendResource();
        ability.DoAction();
    }

    private void GoBackToIdle()
    {
        if (BackToIdleCoroutine != null)
            StopCoroutine(BackToIdleCoroutine);

        NavMeshAgent.ResetPath();
        BackToIdleCoroutine = StartCoroutine(GoBackToIdleCoroutine());
    }

    private IEnumerator GoBackToIdleCoroutine()
    {
        yield return new WaitForSeconds(Ability.Delay);

        if (!PawnState.AbleToFight)
            yield break;
        
        Ability = null;

        CharacterController.SetAnimationState(new IdleState());

        RealizeTurn();
    }

    private void FixedUpdate()
    {
        if (Pawn != null && Pawn.IsAlive)
        {
            Pawn.TickAllBuffs();
        }
        
        if (Ability == null || !PawnState.CanWalk)
            return;

        if (Ability.FocusedPawn == null || !Ability.FocusedPawn.PawnState.CanBeTargeted)
            return;

        if (!PawnState.CanTransition)
            return;

        CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude / NavMeshAgent.speed);

        var direction = Ability.WalkingDestination - transform.position;

        if (!Ability.ShouldUse())
        {
            NavMeshAgent.SetDestination(Ability.WalkingDestination);
            CharacterController.SetDirection(direction);
            CharacterController.SetAnimationState(new IdleState());
        }
        else
        {
            Ability.Used = true;
            NavMeshAgent.SetDestination(transform.position);

            CharacterController.SetAnimationState(new AbilityState(Ability, DoAbility, GoBackToIdle));
        }
    }

    public void FinishBattle()
    {
        Pawn.FinishBattle();

        CharacterController.SetAnimationState(new IdleState());

        enabled = false;
        BattleController = null;
    }

    public void Dance()
    {
        CharacterController.SetAnimationState(new DanceState());
        NavMeshAgent.ResetPath();
    }

    public void SpawnProjectile(
        ProjectileController projectilePrefab,
        AnimationCurve trajectory,
        List<AbilityEffect> effects,
        PawnController focusedPawn,
        bool overrideSprite)
    {
        var roomScene = FindObjectOfType<RoomController>();
        
        var projectile = Instantiate(
                projectilePrefab,
                CharacterController.SpawnPoint.position, 
                Quaternion.identity, 
                roomScene.transform)
            .Init(this, effects, focusedPawn.transform.position, trajectory, Pawn.RangedAttackError);

        if (overrideSprite)
        {
            projectile.OverrideSprite(CharacterController.GetProjectileSprite());
        }
    }

    private void ReceiveAttack()
    {
        CharacterController.DoHitStop();

        if (Pawn.IsAlive)
            return;

        CharacterController.SetAnimationState(new DeadState());
        Ability = null;

        Application.Instance.GetManager<BattleEventsManager>().DoPawnDeathEvent(BattleController.Battle, this);
    }

    private void ReceiveHeal()
    {
        CharacterController.DoNiceHitStop();

        if (!Pawn.IsAlive || PawnState is not DeadState)
            return;
        
        CharacterController.SetAnimationState(new IdleState());
    }

    public void SummonPawn(Pawn pawn)
    {
        var prefab = Application.Instance.GetManager<PartyManager>().BasePawnPrefab;
            
        var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
        var pawnController = Instantiate(prefab, transform.position + randomRotation, Quaternion.identity, BattleController.transform);
        pawnController.Init(pawn);
        
        BattleController.AddPawn(pawnController, pawn.Team);
        pawnController.StartBattle(BattleController, BattleController.Battle);
        pawnController.RealizeTurn();
    }
    
    private void OnDestroy()
    {
        Pawn.LostLife -= ReceiveAttack;
        Pawn.GainedLife -= ReceiveHeal;
    }
}
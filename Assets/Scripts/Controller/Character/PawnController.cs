using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] public PawnCanvasController PawnCanvasController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }

    public Pawn Pawn { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public AnimationState PawnState => CharacterController.CurrentState;
    private Coroutine BackToIdleCoroutine { get; set; }
    private Ability Ability { get; set; }
    private BattleController BattleController { get; set; }

    public void Init(Pawn pawn)
    {
        Pawn = pawn;
        CharacterController = Instantiate(pawn.Character, transform);

        if (pawn.Weapon != null)
        {
            CharacterController.SetWeapon(pawn.Weapon);
        }
        
        Pawn.AbilitySelected += RealizaHabilidade;
    }

    public void StartBattle(BattleController battleController)
    {
        Pawn.StartBattle();

        enabled = true;
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        Ability = null;
        
        BattleController = battleController;
    }

    public IEnumerator RealizaTurno()
    {
        if (Ability != null)
            yield break;
        
        Ability = Pawn.GetCurrentAttackIntent(this, BattleController.Battle);
        Ability.ChooseFocus(BattleController.Battle);

        RealizaHabilidade(Ability);
    }
    
    private void RealizaHabilidade(Ability ability)
    {
        if (ability.IsSpecial && Ability is {IsSpecial: false})
        {
            Ability = ability;
            CharacterController.SetAnimationState(new IdleState());
            return;
        }
        
        Ability = ability;
        CharacterController.SetAnimationState(new IdleState());
    }

    private void DoAbility(Ability ability)
    {
        if (!PawnState.AbleToFight)
            return;

        if (ability.IsSpecial)
        {
            Application.Instance.BattleEventsManager.DoSpecialAttackEvent(BattleController.Battle, this, ability);
        }
        else
        {
            Application.Instance.BattleEventsManager.DoAttackEvent(BattleController.Battle, this, ability);
        }

        Application.Instance.AudioManager.PlaySound(SfxType.Slash);

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

        Pawn.SetInitiative(Ability.Delay);

        Ability = null;
        
        CharacterController.SetAnimationState(new IdleState());
        
        NavMeshAgent.isStopped = true;
        NavMeshAgent.ResetPath();
    }

    private void Update()
    {
        if (Ability == null || !PawnState.CanWalk)
            return;

        if(Ability.FocusedPawn == null || !Ability.FocusedPawn.PawnState.CanBeTargeted)
            return;
        
        var direction = Ability.WalkingDestination - transform.position;
        
        CharacterController.SetDirection(direction);
        CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);

        if (!Ability.ShouldUse())
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Ability.WalkingDestination);
            CharacterController.SetAnimationState(new IdleState());
        }
        else
        {
            Ability.Used = true;
            NavMeshAgent.isStopped = true;
            NavMeshAgent.ResetPath();
            
            CharacterController.SetAnimationState(
                new AbilityState(Ability,  DoAbility, GoBackToIdle));
        }
    }

    private void FixedUpdate()
    {
        if (Pawn == null || !Pawn.IsAlive)
            return;

        Pawn.TickAllBuffs();
    }

    public void FinishBattle()
    {
        Pawn?.RemoveAllBuffs();

        CharacterController.SetAnimationState(new IdleState());
        
        enabled = false;
        BattleController = null;
    }

    public void Dance()
    {
        CharacterController.SetAnimationState(new DanceState());
        NavMeshAgent.isStopped = true;
        NavMeshAgent.ResetPath();
    }

    public void SpawnProjectile(
        ProjectileController projectile,
        List<AbilityEffect> effects,
        PawnController focusedPawn)
    {
        var weaponPosition = CharacterController.WeaponController.SpawnPoint.position;

        var direction = focusedPawn.transform.position - weaponPosition;
        direction = new Vector3(direction.x, 0, direction.z);

        Instantiate(projectile, weaponPosition, Quaternion.LookRotation(direction)).Init(this, effects, direction);
    }

    public void ReceiveAttack()
    {
        CharacterController.DoHitStop();

        if (Pawn.IsAlive) return;

        CharacterController.SetAnimationState(new DeadState());
        NavMeshAgent.isStopped = true;
        Ability = null;
        
        Application.Instance.BattleEventsManager.DoPawnDeathEvent(BattleController.Battle, this);
    }

    public void ReceiveHeal(bool canRevive)
    {
        CharacterController.DoNiceHitStop();

        if (!Pawn.IsAlive || !canRevive)
            return;

        CharacterController.SetAnimationState(new IdleState());
    }

    public void ReceiveBuff(Buff buff)
    {
        Debug.Log(buff.Id);
    }
}
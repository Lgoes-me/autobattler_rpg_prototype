using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] public PlayerFollowController PlayerFollowController { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] public PawnCanvasController PawnCanvasController { get; set; }
    [field: SerializeField] public CharacterController CharacterController { get; set; }

    public Pawn Pawn { get; private set; }
    public AnimationState PawnState => CharacterController.CurrentState;
    private Coroutine BackToIdleCoroutine { get; set; }
    private Ability Ability { get; set; }

    public void Init(Pawn pawn)
    {
        Pawn = pawn;
        
        CharacterController = Instantiate(pawn.Character, transform);

        if (pawn.Weapon != null)
            CharacterController.SetWeapon(pawn.Weapon);

        if (TryGetComponent<PlayerFollowController>(out var playerFollowController))
        {
            playerFollowController.CharacterController = CharacterController;
        }

        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.CharacterController = CharacterController;
        }

        if (TryGetComponent<EnemyController>(out var enemyController))
        {
            enemyController.CharacterController = CharacterController;
        }
    }

    public void StartBattle()
    {
        Pawn.StartBattle();
        
        enabled = true;
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        Ability = null;
    }

    public IEnumerator PawnTurn(Battle battle)
    {
        if (Ability == null)
        {
            Ability = Pawn.GetCurrentAttackIntent(this, battle, Pawn.Team is TeamType.Enemies);
            Ability.ChooseFocus(battle);
        }

        if (!Ability.ShouldUse())
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Ability.WalkingDestination);
            CharacterController.SetAnimationState(new IdleState());
        }
        else
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.SetDestination(transform.position);
            CharacterController.SetAnimationState(new AbilityState(Ability, DoAbility), GoBackToIdle);
        }

        yield break;
    }

    private void DoAbility()
    {
        if (Ability == null || !PawnState.AbleToFight)
            return;

        if (Ability.IsSpecial)
        {
            Application.Instance.BattleEventsManager.DoSpecialAttackEvent(this, Ability);
        }
        else
        {
            Application.Instance.BattleEventsManager.DoAttackEvent(this, Ability);
        }

        Application.Instance.AudioManager.PlaySound(SfxType.Slash);

        Ability.SpendResource();
        Ability.DoAction();
    }

    private void GoBackToIdle()
    {
        if (BackToIdleCoroutine != null)
            StopCoroutine(BackToIdleCoroutine);

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
    }

    private void Update()
    {
        if (Ability == null || !PawnState.CanWalk)
            return;

        CharacterController.SetDirection(NavMeshAgent.velocity);

        if (Ability.ShouldUse())
        {
            NavMeshAgent.isStopped = true;
            CharacterController.SetSpeed(0);
        }
        else if (NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && NavMeshAgent.remainingDistance < 1f)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Ability.WalkingDestination);
            CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
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
    }

    public void Dance()
    {
        CharacterController.SetAnimationState(new DanceState());
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
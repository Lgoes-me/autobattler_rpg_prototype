using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] public PawnData PawnData { get; private set; }
    [field: SerializeField] public PlayerFollowController PlayerFollowController { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] public PawnCanvasController PawnCanvasController { get; set; }
    [field: SerializeField] public CharacterController CharacterController { get; set; }

    [field: SerializeField] public TeamType Team { get; private set; }

    public PawnDomain Pawn { get; private set; }
    public AnimationState PawnState => CharacterController.CurrentState;
    private Coroutine BackToIdleCoroutine { get; set; }
    private Ability Ability { get; set; }
    private Ability RequestedSpecialAbility { get; set; }

    public PawnController Init()
    {
        Pawn = PawnData.ToDomain();

        if (Application.Instance.Save.SelectedParty.TryGetValue(PawnData.name, out var pawnInfo))
        {
            Pawn.SetPawnInfo(pawnInfo);
        }
        else if (Application.Instance.Save.PlayerPawn.PawnName == PawnData.name)
        {
            Pawn.SetPawnInfo(Application.Instance.Save.PlayerPawn);
        }

        enabled = true;
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        Ability = null;
        RequestedSpecialAbility = null;

        return this;
    }

    public IEnumerator PawnTurn(Battle battle)
    {
        if (Ability == null)
        {
            Ability = RequestedSpecialAbility ??
                      Pawn.GetCurrentAttackIntent(this, battle, Team is TeamType.Enemies).ToDomain(this);
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

        if (Ability == RequestedSpecialAbility)
            RequestedSpecialAbility = null;

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

    public void Deactivate()
    {
        Pawn?.RemoveAllBuffs();

        CharacterController.SetAnimationState(new IdleState());
        enabled = false;
    }

    public void DoSpecial(AbilityData attackData)
    {
        RequestedSpecialAbility = attackData.ToDomain(this);
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

    public void SetCharacter(PawnData pawnData)
    {
        PawnData = pawnData;
        CharacterController = Instantiate(pawnData.Character, transform);

        if (pawnData.Weapon != null)
            CharacterController.SetWeapon(pawnData.Weapon);

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

    public void ReceiveAttack()
    {
        CharacterController.DoHitStop();
        PawnCanvasController.UpdateLife();

        if (Pawn.IsAlive) return;

        CharacterController.SetAnimationState(new DeadState());
        NavMeshAgent.isStopped = true;
        Ability = null;
    }

    public void ReceiveHeal(bool canRevive)
    {
        CharacterController.DoNiceHitStop();
        PawnCanvasController.UpdateLife();

        if (!Pawn.IsAlive || !canRevive)
            return;

        CharacterController.SetAnimationState(new IdleState());
    }

    public void ReceiveBuff(Buff buff)
    {
        Debug.Log(buff.Id);
    }
}
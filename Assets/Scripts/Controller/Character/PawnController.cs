using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] private BasePawnCanvasController CanvasController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }

    public Pawn Pawn { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public AnimationState PawnState => CharacterController.CurrentState;
    private Coroutine BackToIdleCoroutine { get; set; }
    private Ability Ability { get; set; }
    public BattleController BattleController { get; private set; }

    public void Init(Pawn pawn)
    {
        Pawn = pawn;
        CharacterController = Instantiate(pawn.Character, transform);

        if (CanvasController != null)
        {
            CanvasController.Init(this);
        }

        if (pawn.Weapon != null)
        {
            CharacterController.SetWeapon(pawn.Weapon);
        }
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
        NavMeshAgent.isStopped = true;

        Ability = null;

        BattleController = battleController;
        CharacterController.SetAnimationState(new IdleState());
    }

    public void RealizaTurno()
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

        NavMeshAgent.isStopped = true;
        NavMeshAgent.ResetPath();

        RealizaTurno();
    }

    private void Update()
    {
        if (Ability == null || !PawnState.CanWalk)
            return;

        if (Ability.FocusedPawn == null || !Ability.FocusedPawn.PawnState.CanBeTargeted)
            return;
        
        CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude/NavMeshAgent.speed);

        var direction = Ability.WalkingDestination - transform.position;
        CharacterController.SetDirection(direction);
        
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

            CharacterController.SetAnimationState(new AbilityState(Ability, DoAbility, GoBackToIdle));
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
        Pawn.FinishBattle();

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
        AnimationCurve trajectory,
        List<AbilityEffect> effects,
        PawnController focusedPawn)
    {
        var weaponPosition = CharacterController.WeaponController?.SpawnPoint.position ?? CharacterController.Hand.position;
        var destination = focusedPawn.transform.position;

        Instantiate(projectile, weaponPosition, Quaternion.identity).Init(this, effects, destination, trajectory);
    }

    public void ReceiveAttack()
    {
        CharacterController.DoHitStop();

        if (Pawn.IsAlive) return;

        CharacterController.SetAnimationState(new DeadState());
        NavMeshAgent.isStopped = true;
        Ability = null;

        Application.Instance.GetManager<BattleEventsManager>().DoPawnDeathEvent(BattleController.Battle, this);
    }

    public void ReceiveHeal(bool canRevive)
    {
        CharacterController.DoNiceHitStop();

        if (!Pawn.IsAlive || !canRevive)
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
        pawnController.RealizaTurno();
    }

    public void ReceiveBuff(Buff buff)
    {
        Debug.Log(buff.Id);
    }
}
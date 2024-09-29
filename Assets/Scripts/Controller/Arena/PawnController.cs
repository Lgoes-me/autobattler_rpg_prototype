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
    [field: SerializeField] private CharacterController CharacterController { get; set; }
    
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

    public IEnumerator Turno(List<PawnController> pawns)
    {
        Ability = RequestedSpecialAbility ?? Pawn.GetCurrentAttackIntent().ToDomain(this);
        Ability.ChooseFocus(pawns);

        var direction = Ability.Destination - transform.position;
        
        if (direction.magnitude > Ability.Range)
        {
            CharacterController.SetAnimationState(new IdleState());
        }
        else
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.SetDestination(transform.position);
            CharacterController.SetSpeed(0);
            CharacterController.SetDirection(direction);
            
            CharacterController.SetAnimationState(new AttackState(Ability, AttackEnemy), GoBackToIdle);
        }
        
        yield break;
    }

    private void AttackEnemy()
    {
        if(Ability == null || !PawnState.AbleToFight)
            return;

        if(!Ability.HasResource())
            return;

        if (Ability == RequestedSpecialAbility)
            RequestedSpecialAbility = null;

        Ability.SpendResource();
        
        Application.Instance.AudioManager.PlaySound(SfxType.Slash);

        Ability.DoAction();
    }
    
    private void GoBackToIdle()
    {
        if(BackToIdleCoroutine != null)
            StopCoroutine(BackToIdleCoroutine);
        
        BackToIdleCoroutine = StartCoroutine(GoBackToIdleCoroutine());
    }
    
    private IEnumerator GoBackToIdleCoroutine()
    {
        yield return new WaitForSeconds(Ability.Delay);

        if (!PawnState.AbleToFight)
            yield break;
        
        CharacterController.SetAnimationState(new IdleState());
    }

    public void ReceiveAttack()
    {
        var dead = Pawn.Health <= 0;
        CharacterController.DoHitStop();
        PawnCanvasController.UpdateLife();

        if (!dead) return;
        
        CharacterController.SetAnimationState(new DeadState());
        NavMeshAgent.isStopped = true;
        Ability = null;
    }

    private void Update()
    {
        if (Ability == null || !PawnState.CanWalk)
            return;

        CharacterController.SetDirection(NavMeshAgent.velocity);
        var direction = Ability.Destination - transform.position;
        
        if (Ability != null && Ability.Range >= direction.magnitude)
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.SetDestination(transform.position);
            CharacterController.SetSpeed(0);
        }
        else if(NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && NavMeshAgent.remainingDistance < 1f)
        {
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * (Ability.Range - 1);
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Ability.Destination + randomRotation);
            CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
        }
    }

    public void Deactivate()
    {
        CharacterController.SetAnimationState(new IdleState());
        enabled = false;
    }

    public void DoSpecial(AttackData attackData)
    {
        RequestedSpecialAbility = attackData.ToDomain(this);
    }
    
    public void Dance()
    {
        PawnCanvasController.Hide();
        CharacterController.SetAnimationState(new DanceState());
    }

    public void SpawnProjectile(ProjectileController projectile, AbilityEffect effect)
    {
        var direction = Ability.Destination - CharacterController.SpawnPoint.position;
        direction = new Vector3(direction.x, 0, direction.z);
            
        Instantiate(projectile, CharacterController.SpawnPoint.position, Quaternion.LookRotation(direction)).Init(this, effect, direction);
    }
    
    public void UpdateMana()
    {
        PawnCanvasController.UpdateMana();
    }

    public void SetCharacter(PawnData pawnData)
    {
        PawnData = pawnData;
        CharacterController = Instantiate(pawnData.Character, transform);
        
        if(TryGetComponent<PlayerFollowController>(out var playerFollowController))
        {
            playerFollowController.CharacterController = CharacterController;
        }
        
        if(TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.CharacterController = CharacterController;
        }
        
        if(TryGetComponent<EnemyController>(out var enemyController))
        {
            enemyController.CharacterController = CharacterController;
        }
    }
}
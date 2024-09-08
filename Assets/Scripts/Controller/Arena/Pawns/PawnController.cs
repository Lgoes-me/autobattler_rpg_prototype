using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] public PawnData PawnData { get; private set; }
    
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    
    [field: SerializeField] private CanvasFollowController CanvasFollowController { get; set; }
    [field: SerializeField] private PawnCanvasController PawnCanvasController { get; set; }
    
    [field: SerializeField] private AnimationStateController AnimationStateController { get; set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private SpriteRenderer Body { get; set; }
    
    [field: SerializeField] private TeamType Team { get; set; }

    public PawnDomain Pawn { get; private set; }
    public AnimationState PawnState => AnimationStateController.CurrentState;
    private Attack Attack { get; set; }
    private PawnController Focus { get; set; }
    private Coroutine BackToIdleCoroutine { get; set; }
    private bool SpecialAttackRequested { get; set; }

    public PawnController Init()
    {
        Pawn = PawnData.ToDomain();

        enabled = true;
        NavMeshAgent.enabled = true;
        CanvasFollowController.Show();
        Attack = null;
        Focus = null;

        return this;
    }

    public IEnumerator Turno(List<PawnController> basePawnControllers)
    {
        if (Focus == null || !Focus.PawnState.CanBeTargeted)
        {
            var closest =
                basePawnControllers
                    .Where(pawn => pawn.Team != Team && pawn.PawnState.CanBeTargeted)
                    .OrderBy(pawn => (pawn.transform.position - transform.position).sqrMagnitude)
                    .Take(3)
                    .ToList();

            if (closest.Count == 0)
                yield break;

            Focus = closest[Random.Range(0, closest.Count)];
        }

        var direction = Focus.transform.position - transform.position;
        Attack = Pawn.GetCurrentAttackIntent();

        if (direction.magnitude > Attack.Range)
        {
            AnimationStateController.SetAnimationState(new IdleState());
        }
        else if(SpecialAttackRequested)
        {
            AnimationStateController.SetAnimationState(new SpecialAttackState(Pawn.SpecialAttack, SpecialAttackEnemy), GoBackToIdle);
        }
        else
        {
            AnimationStateController.SetAnimationState(new AttackState(Attack, AttackEnemy), GoBackToIdle);
        }
    }

    private void AttackEnemy()
    {
        if(Focus == null || !PawnState.AbleToFight)
            return;
        
        if (Team == TeamType.Player)
        {
            Pawn.Mana = Mathf.Clamp(Pawn.Mana + 10, 0, Pawn.MaxMana);
            PawnCanvasController.UpdateMana(!SpecialAttackRequested);
        }

        Focus.ReceiveAttack(Attack.Damage);
    }
    
    private void SpecialAttackEnemy()
    {
        if(Focus == null || !PawnState.AbleToFight)
            return;
        
        SpecialAttackRequested = false;
        Pawn.Mana = 0;
        PawnCanvasController.UpdateMana(!SpecialAttackRequested);

        Focus.ReceiveAttack(Attack.Damage);
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

    private void ReceiveAttack(int attack)
    {
        Pawn.Health = Mathf.Clamp(Pawn.Health - attack, 0, Pawn.MaxHealth);
        var dead = Pawn.Health <= 0;
        
        PawnCanvasController.UpdateLife(!dead);

        if (!dead) return;
        
        CanvasFollowController.Hide();
        AnimationStateController.SetAnimationState(new DeadState());
        NavMeshAgent.isStopped = true;
        Focus = null;
    }

    private void Update()
    {
        Body.flipX = NavMeshAgent.velocity.x < 0;
        
        if (Focus == null || !PawnState.CanWalk)
            return;

        var direction = Focus.transform.position - transform.position;
        if (Attack != null && Attack.Range >= direction.magnitude)
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.SetDestination(transform.position);
            Animator.SetFloat("Speed", 0f);
        }
        else
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Focus.transform.position);
            Animator.SetFloat("Speed", NavMeshAgent.velocity.magnitude);
        }
    }

    public void Deactivate()
    {
        CanvasFollowController.Hide();
        enabled = false;
    }

    public void DoSpecial()
    {
        SpecialAttackRequested = true;
    }
    
    public void Dance()
    {
        CanvasFollowController.Hide();
        AnimationStateController.SetAnimationState(new DanceState(), GoBackToIdle);
    }
}
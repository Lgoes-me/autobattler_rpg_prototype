using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer Body { get; set; }
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private CanvasFollowController CanvasFollowController { get; set; }
    [field: SerializeField] private PawnCanvasController PawnCanvasController { get; set; }
    [field: SerializeField] private AnimationStateMachine AnimationStateMachine { get; set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private TeamType Team { get; set; }

    public PawnDomain Pawn { get; private set; }
    public AnimationState PawnState => AnimationStateMachine.CurrentState;
    private Attack Attack { get; set; }
    private PawnController Focus { get; set; }

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
            AnimationStateMachine.SetAnimationState(new IdleState());
        }
        else
        {
            AnimationStateMachine.SetAnimationState(new AttackState(Attack), () => AttackEnemy(Focus));
        }
    }

    private async void AttackEnemy(PawnController enemy)
    {
        if (Team == TeamType.Player)
        {
            Pawn.Mana = Mathf.Clamp(Pawn.Mana + 10, 0, Pawn.MaxMana);
            PawnCanvasController.UpdateMana();
        }

        enemy.ReceiveAttack(Attack.Damage);

        await Task.Delay(Attack.Delay);

        AnimationStateMachine.SetAnimationState(new IdleState());
    }

    private async void ReceiveAttack(int attack)
    {
        Pawn.Health -= attack;
        await PawnCanvasController.UpdateLife();

        if (Pawn.Health <= 0)
        {
            CanvasFollowController.Hide();
            AnimationStateMachine.SetAnimationState(new DeadState());
            NavMeshAgent.isStopped = true;
            Focus = null;
        }
    }

    private void Update()
    {
        Body.flipX = NavMeshAgent.velocity.x < 0;
        
        if (Focus == null || PawnState is not IdleState)
            return;

        var direction = Focus.transform.position - transform.position;
        if (direction.magnitude <= Attack.Range)
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
        
    }
}
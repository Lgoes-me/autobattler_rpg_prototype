using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private CanvasFollowController CanvasFollowController { get; set; }
    [field: SerializeField] private PawnCanvasController PawnCanvasController { get; set; }
    [field: SerializeField] private AnimationStateMachine AnimationStateMachine { get; set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private TeamType Team { get; set; }

    private ArenaController ArenaController { get; set; }
    public PawnDomain Pawn { get; private set; }
    public AnimationState PawnState => AnimationStateMachine.CurrentState;
    private PawnController Focus { get; set; }

    public PawnController Init(ArenaController arenaController, PawnDomain pawn)
    {
        ArenaController = arenaController;
        Pawn = pawn;

        enabled = true;
        NavMeshAgent.enabled = true;
        CanvasFollowController.Show();
        
        return this;
    }

    public IEnumerator Turno(List<PawnController> basePawnControllers)
    {
        var closest =
            basePawnControllers
                .Where(pawn => pawn.Team != Team && pawn.PawnState.CanBeTargeted)
                .OrderBy(pawn => (pawn.transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault();

        if (closest == null)
            yield break;

        Focus = closest;
        var direction = Focus.transform.position - transform.position;
        
        if (direction.magnitude > Pawn.AttackRange)
        {
            AnimationStateMachine.SetAnimationState(new IdleState());
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(direction, transform.up);
            AnimationStateMachine.SetAnimationState(new AttackState(), () => AttackEnemy(Focus));
        }
    }

    private void AttackEnemy(PawnController enemy)
    {
        Pawn.Mana += 10;
        PawnCanvasController.UpdateMana();

        enemy.ReceiveAttack(Pawn.Attack);

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

    private void FixedUpdate()
    {
        if (Focus == null || PawnState is not IdleState)
            return;

        var direction = Focus.transform.position - transform.position;
        if (direction.magnitude <= Pawn.AttackRange)
        {
            NavMeshAgent.isStopped = true;
            Focus = null;
            Animator.SetFloat("Speed", 0f);            
            transform.rotation = Quaternion.LookRotation(direction, transform.up);
        }
        else
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Focus.transform.position);
            Animator.SetFloat("Speed", NavMeshAgent.velocity.magnitude);
            transform.rotation = transform.rotation.Rotate(Quaternion.LookRotation(NavMeshAgent.velocity, transform.up), 25);
        }
    }

    public void Deactivate()
    {
        CanvasFollowController.Hide();
        enabled = false;
    }
}
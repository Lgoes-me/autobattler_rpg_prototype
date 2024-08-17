using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] protected NavMeshAgent NavMeshAgent { get; private set; }
    [field: SerializeField] private Transform Visuals { get; set; }
    [field: SerializeField] private TeamType Team { get; set; }
    
    private ArenaController ArenaController { get; set; }
    public PawnDomain Pawn { get; protected set; }
    private PawnController Focus { get; set; }

    public void Init(ArenaController arenaController, PawnDomain pawn)
    {
        ArenaController = arenaController;
        Pawn = pawn;
    }

    public IEnumerator Turno(List<PawnController> basePawnControllers)
    {
        var closestEnemies =
            basePawnControllers
                .Where(pawn => pawn.Team != Team && pawn.Pawn.State != StateType.Dead)
                .OrderBy(pawn => (pawn.transform.position - transform.position).sqrMagnitude)
                .Take(3)
                .ToList();

        var closest = closestEnemies[Random.Range(0, closestEnemies.Count)];

        if (closest == null)
            yield break;

        Focus = closest;

        if ((Focus.transform.position - transform.position).magnitude > Pawn.AttackRange)
        {
            Pawn.State = StateType.Move;
        }
        else
        {
            Pawn.State = StateType.Attack;
            DoAttack(Focus);
        }
    }

    private void DoAttack(PawnController enemy)
    {
        StartCoroutine(AttackCoroutine(enemy));
    }

    private IEnumerator AttackCoroutine(PawnController enemy)
    {
        Visuals.localPosition = Visuals.localPosition + Vector3.up * 0.5f;

        yield return new WaitForSeconds(0.1f);

        Visuals.localPosition = Vector3.up;

        if (Pawn.State is not StateType.Attack)
        {
            Focus = null;
            yield break;
        }

        enemy.ReceiveAttack(Pawn.Attack);
        Pawn.State = StateType.Idle;
        Focus = null;
    }

    private void ReceiveAttack(int attack)
    {
        Pawn.Health -= attack;

        if (Pawn.Health <= 0)
        {
            Visuals.gameObject.SetActive(false);
            Pawn.State = StateType.Dead;
        }
    }

    private void FixedUpdate()
    {
        if (Focus == null || Pawn.State is not StateType.Move)
            return;

        if ((Focus.transform.position - transform.position).magnitude <= Pawn.AttackRange)
        {
            NavMeshAgent.isStopped = true;
            Focus = null;
            Pawn.State = StateType.Idle;
        }
        else
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Focus.transform.position);
        }
    }
}
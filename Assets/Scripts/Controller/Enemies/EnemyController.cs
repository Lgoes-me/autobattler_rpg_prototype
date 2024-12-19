using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public PawnController PawnController { get; private set; }
    
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private List<Transform> Nodes { get; set; }

    private EnemyAreaController EnemyAreaController { get; set; }

    private int CurrentNode { get; set; }
    private bool Following { get; set; }

    public void Activate(EnemyAreaController enemyAreaController)
    {
        EnemyAreaController = enemyAreaController;
        Following = false;

        if (Nodes.Count > 0)
        {
            NavMeshAgent.SetDestination(Nodes[CurrentNode].transform.position);
        }
    }
    
    private void Update()
    {
        PawnController.CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
        PawnController.CharacterController.SetDirection(NavMeshAgent.velocity);
        
        var player = Application.Instance.PlayerManager.PlayerController;
        var distance = player.transform.position - transform.position;
        
        if (Following)
        {
            NavMeshAgent.SetDestination(player.transform.position);
        }
        else if (Nodes.Count > 0 &&
                 NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                 NavMeshAgent.remainingDistance < 1f)
        {
            CurrentNode++;

            if (CurrentNode >= Nodes.Count)
            {
                CurrentNode = 0;
            }

            NavMeshAgent.SetDestination(Nodes[CurrentNode].transform.position);
        }

        switch (distance.sqrMagnitude)
        {
            case < 2f:
                NavMeshAgent.SetDestination(transform.position);
                NavMeshAgent.isStopped = true;
                EnemyAreaController.StartBattle();
                break;
            case < 15f:
                Following = true;
                break;
            default:
                Following = false;
                break;
        }
    }

    public void Prepare()
    {
        PawnController.CharacterController.SetSpeed(0);
        enabled = false;
    }
}
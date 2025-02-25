using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public PawnController PawnController { get; private set; }
    
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private List<Transform> Nodes { get; set; }

    private EnemyAreaController EnemyAreaController { get; set; }

    private bool Initialized { get; set; }
    private int CurrentNode { get; set; }
    private bool Following { get; set; }

    public void Init(EnemyAreaController enemyAreaController)
    {
        EnemyAreaController = enemyAreaController;
        Initialized = true;
        Following = false;

        if (Nodes.Count > 0)
        {
            NavMeshAgent.SetDestination(Nodes[CurrentNode].transform.position);
        }
    }
    
    private void Update()
    {
        if(!Initialized)
            return;
        
        PawnController.CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
        PawnController.CharacterController.SetDirection(NavMeshAgent.velocity);
        
        var player = Application.Instance.PlayerManager.PlayerController;
        var distance = Vector3.Distance(player.transform.position, transform.position);
        
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

        if (distance < PawnController.Pawn.AttackRange)
        {
            NavMeshAgent.SetDestination(transform.position);
            NavMeshAgent.isStopped = true;

            EnemyAreaController.StartBattle();
        }
        else if (distance < PawnController.Pawn.VisionRange)
        {
            Following = true;
        }
        else if (distance > PawnController.Pawn.VisionRange)
        {
            Following = false;
        }
    }

    public void Prepare()
    {
        PawnController.CharacterController.SetSpeed(0);
        enabled = false;
    }
}
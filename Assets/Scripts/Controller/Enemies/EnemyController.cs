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
        NavMeshAgent.enabled = true;
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
        
        var player = Application.Instance.GetManager<PlayerManager>().PlayerTransform;
        var distance = Vector3.Distance(player.position, transform.position);
        
        if (Following)
        {
            NavMeshAgent.SetDestination(player.position);
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
        if (distance < PawnController.Pawn.GetComponent<EnemyComponent>().AttackRange)
        {
            NavMeshAgent.ResetPath();

            EnemyAreaController.StartBattle();
        }
        else if (distance < PawnController.Pawn.GetComponent<EnemyComponent>().VisionRange)
        {
            Following = true;
        }
        else if (distance > PawnController.Pawn.GetComponent<EnemyComponent>().VisionRange)
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
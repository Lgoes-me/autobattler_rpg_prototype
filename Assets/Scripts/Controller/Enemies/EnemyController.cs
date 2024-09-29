using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; set; }
    [field: SerializeField] public PawnController PawnController { get; set; }
    
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private List<Transform> Nodes { get; set; }

    private Action OnPlayerCollision { get; set; }

    private int CurrentNode { get; set; }
    private bool Following { get; set; }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate(Action onPlayerCollision)
    {
        OnPlayerCollision = onPlayerCollision;

        Following = false;
        gameObject.SetActive(true);

        if (Nodes.Count > 0)
        {
            NavMeshAgent.SetDestination(Nodes[CurrentNode].transform.position);
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
        CharacterController.SetDirection(NavMeshAgent.velocity);
        
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
                transform.position -= distance * 0.25f;
                NavMeshAgent.enabled = false;
                OnPlayerCollision();
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
        CharacterController.SetSpeed(0);
        NavMeshAgent.enabled = false;
        enabled = false;
    }
}
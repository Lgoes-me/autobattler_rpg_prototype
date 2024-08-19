using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public PawnData PawnData { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] private List<Transform> Nodes { get; set; }

    private PlayerController Player { get; set; }
    private Action OnPlayerCollision { get; set; }

    private int CurrentNode { get; set; }
    private bool Following { get; set; }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate(PlayerController player, Action onPlayerCollision)
    {
        Player = player;
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
        Player = null;
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        SetDestination();

        if (Player == null)
            return;

        var distance = Player.transform.position - transform.position;

        switch (distance.sqrMagnitude)
        {
            case < 2f:
                transform.position -= distance * 0.25f;
                NavMeshAgent.enabled = false;
                OnPlayerCollision();
                break;
            case < 15f when Vector3.Dot(distance, transform.forward) > 1f:
                Following = true;
                break;
            default:
                Following = false;
                break;
        }
    }

    private void SetDestination()
    {
        if (Following)
        {
            NavMeshAgent.SetDestination(Player.transform.position);
        }
        else if (Nodes.Count > 0 &&
                 NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                 NavMeshAgent.remainingDistance < 0.5f)
        {
            CurrentNode++;

            if (CurrentNode >= Nodes.Count)
            {
                CurrentNode = 0;
            }

            NavMeshAgent.SetDestination(Nodes[CurrentNode].transform.position);
        }
    }
}
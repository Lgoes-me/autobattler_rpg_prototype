using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerFollowController : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
    private Coroutine FollowCoroutine { get; set; }

    public void StartFollow(PawnController toFollow, Vector3 position)
    {
        CharacterController.SetAnimationState(new IdleState());
        
        if (FollowCoroutine != null)
        {
            StopCoroutine(FollowCoroutine);
        }

        if (position != Vector3.zero)
        {
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 0.5f;
            transform.position = position + randomRotation;
        }
        
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = false;
        FollowCoroutine = StartCoroutine(DoFollowCoroutine(toFollow));
    }

    private IEnumerator DoFollowCoroutine(PawnController toFollow)
    {
        while (gameObject.activeInHierarchy)
        {
            var destination = toFollow.transform.position;
            yield return new WaitForSeconds(0.25f);

            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 0.5f;
            CharacterController.SetDirection(destination - transform.position);
            NavMeshAgent.SetDestination(destination + randomRotation);
        }
    }

    public void StopFollow()
    {
        CharacterController.SetAnimationState(new IdleState());
        
        if (FollowCoroutine != null)
        {
            StopCoroutine(FollowCoroutine);
        }

        if (!NavMeshAgent.enabled)
            return;

        NavMeshAgent.SetDestination(transform.position);
        NavMeshAgent.isStopped = true;
        NavMeshAgent.enabled = false;
    }

    private void Update()
    {
        if (FollowCoroutine == null)
            return;

        CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude);
    }
}
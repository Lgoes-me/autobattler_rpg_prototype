using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public static class MonoBehaviourExtensions
{
    public static Task WaitEndOfFrame(this MonoBehaviour obj)
    {
        var task = new TaskCompletionSource<bool>();
        obj.StartCoroutine(WaitEndOfFrameCoroutine());

        return task.Task;
        
        IEnumerator WaitEndOfFrameCoroutine()
        {
            yield return new WaitForEndOfFrame();
            task.SetResult(true);
        }
    }
    
    public static Task WaitToArriveAtDestination(this MonoBehaviour obj, NavMeshAgent agent, Vector3 destination)
    {
        var task = new TaskCompletionSource<bool>();
        
        agent.enabled = true;
        agent.SetDestination(destination);

        obj.StartCoroutine(WaitToArriveAtDestinationCoroutine());

        return task.Task;
        
        IEnumerator WaitToArriveAtDestinationCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            yield return new WaitUntil(() =>
                agent.pathStatus == NavMeshPathStatus.PathComplete && 
                agent.remainingDistance < 0.5f);
                
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
            
            task.SetResult(true);
        }
    }
}
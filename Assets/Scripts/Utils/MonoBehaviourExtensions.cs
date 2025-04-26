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
    
    public static Task WaitForSeconds(this MonoBehaviour obj, float time)
    {
        var task = new TaskCompletionSource<bool>();
        obj.StartCoroutine(WaitEndOfFrameCoroutine());

        return task.Task;
        
        IEnumerator WaitEndOfFrameCoroutine()
        {
            yield return new WaitForSeconds(time);
            task.SetResult(true);
        }
    }

    public static Task WaitToArriveAtDestination(this MonoBehaviour obj, NavMeshAgent agent, Vector3 destination)
    {
        var task = new TaskCompletionSource<bool>();
        
        agent.SetDestination(destination);

        obj.StartCoroutine(WaitToArriveAtDestinationCoroutine());

        return task.Task;
        
        IEnumerator WaitToArriveAtDestinationCoroutine()
        {
            yield return new WaitForSeconds(0.5f);

            var time = Time.time;
            
            yield return new WaitUntil(() =>
                Time.time - time > 3f ||
                agent.pathStatus == NavMeshPathStatus.PathComplete && 
                agent.remainingDistance < 1f);
                
            agent.isStopped = true;
            agent.ResetPath();
            
            task.SetResult(true);
        }
    }
}
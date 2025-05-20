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
}
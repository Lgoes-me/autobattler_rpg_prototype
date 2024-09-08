using UnityEngine;

public static class Extensions
{
    public static Vector3 Follow(this Vector3 origin, Vector3 destination, int decay)
    {
        return destination + (origin - destination) * Mathf.Exp(-decay * Time.deltaTime);
    }
    public static Quaternion Rotate(this Quaternion from, Quaternion to, float speed)
    {
        return Quaternion.Slerp(from, to, 1 - Mathf.Exp(-speed * Time.deltaTime));
    }
}
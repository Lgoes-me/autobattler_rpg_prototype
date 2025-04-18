using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public static bool IsEnumFlagPresent<T>(this T value, T lookingForFlag) where T : Enum
    {
        return ((int)(object)value & (int)(object)lookingForFlag) != 0;
    }
    
    
#if UNITY_EDITOR
    public static List<T> FindAllScriptableObjectsOfType<T>(string folder = "Assets")
        where T : ScriptableObject
    {
        return AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { folder })
            .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();
    }
#endif
}
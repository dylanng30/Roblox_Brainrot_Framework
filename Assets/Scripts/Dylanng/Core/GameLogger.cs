using UnityEngine;

namespace Dylanng.Core
{
    public static class GameLogger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message) => Debug.Log($"[LOG] {message}");

        [System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object message) => Debug.LogWarning($"[WARNING] {message}");

        [System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(object message) => Debug.LogError($"[ERROR] {message}");
    }
}

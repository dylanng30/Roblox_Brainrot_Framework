using RobloxFW.WorldEventSystem.Data;
using RobloxFW.WorldEventSystem.Manager;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Example
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        public float baseSpeed = 5f;
        
        [Header("References")]
        public WorldEventManager worldEventManager; // In a real setup, you might use a ServiceLocator or Dependency Injection

        private float _currentSpeedMultiplier = 1f;

        // Anti-Memory Leak: ALWAYS subscribe in OnEnable
        private void OnEnable()
        {
            if (worldEventManager != null)
            {
                worldEventManager.OnWorldEventStarted += HandleWorldEventStarted;
                worldEventManager.OnWorldEventEnded += HandleWorldEventEnded;
            }
        }

        // Anti-Memory Leak: ALWAYS unsubscribe in OnDisable or OnDestroy
        private void OnDisable()
        {
            if (worldEventManager != null)
            {
                worldEventManager.OnWorldEventStarted -= HandleWorldEventStarted;
                worldEventManager.OnWorldEventEnded -= HandleWorldEventEnded;
            }
        }

        private void HandleWorldEventStarted(WorldEventEnum eventId)
        {
            if (eventId == WorldEventEnum.SpeedBoost)
            {
                // In a real scenario, you'd fetch the multiplier from the active strategy or a centralized stats manager.
                // For this example, we apply a hardcoded boost or just acknowledge the event.
                _currentSpeedMultiplier = 2f; 
                Debug.Log($"PlayerMovement: Speed Boost applied! Current Multiplier: {_currentSpeedMultiplier}");
            }
        }

        private void HandleWorldEventEnded(WorldEventEnum eventId)
        {
            if (eventId == WorldEventEnum.SpeedBoost)
            {
                _currentSpeedMultiplier = 1f;
                Debug.Log("PlayerMovement: Speed Boost removed. Back to normal speed.");
            }
        }

        private void Update()
        {
            // Simulate movement
            float currentSpeed = baseSpeed * _currentSpeedMultiplier;
            // transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }
}

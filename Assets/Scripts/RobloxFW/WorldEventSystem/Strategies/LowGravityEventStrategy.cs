using System;
using Dylanng.Core;
using RobloxFW.WorldEventSystem.Data;
using RobloxFW.WorldEventSystem.Data.Runtime;
using RobloxFW.WorldEventSystem.Interfaces;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Strategies
{
    [Serializable]
    public class LowGravityEventStrategy : IWorldEventStrategy
    {
        public WorldEventEnum EventId => WorldEventEnum.LowGravity;
        
        public bool IsActive { get; set; }

        private float _multiplier;

        public void Initialize(WorldEventContext context)
        {
            _multiplier = context.Multiplier;
            IsActive = true;
        }

        public void OnEventStart()
        {
            GameLogger.Log($"<color=cyan>[WORLD EVENT SYSTEM]</color> " +
                           $"Low Gravity Started! Gravity Multiplier: {_multiplier}");
            
            Physics.gravity = new Vector3(0, -9.81f * _multiplier, 0);
        }

        public void OnEventTick(float deltaTime)
        {
            if (!IsActive) return;
            // Execute per-frame logic
        }

        public void OnEventEnd()
        {
            GameLogger.Log($"<color=cyan>[WORLD EVENT SYSTEM]</color> " +
                           $"Low Gravity Ended!");
            
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }
}

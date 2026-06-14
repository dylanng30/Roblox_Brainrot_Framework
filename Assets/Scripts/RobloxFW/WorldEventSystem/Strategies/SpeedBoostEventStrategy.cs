using System;
using Dylanng.Core;
using RobloxFW.WorldEventSystem.Data;
using RobloxFW.WorldEventSystem.Data.Runtime;
using RobloxFW.WorldEventSystem.Interfaces;

namespace RobloxFW.WorldEventSystem.Strategies
{
    [Serializable]
    public class SpeedBoostEventStrategy : IWorldEventStrategy
    {
        public WorldEventEnum EventId => WorldEventEnum.SpeedBoost;
        
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
                           $"Speed Boost Started! Multiplier: {_multiplier}");
        }

        public void OnEventTick(float deltaTime)
        {
            if (!IsActive) return;
            // Additional per-frame logic here if needed
        }

        public void OnEventEnd()
        {
            GameLogger.Log($"<color=cyan>[WORLD EVENT SYSTEM]</color> " +
                           $"Speed Boost Ended!");
        }
    }
}

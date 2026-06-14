using System;
using System.Collections.Generic;
using Dylanng.Core;
using Dylanng.Core.Base;
using Dylanng.Core.Systems.TickSystem;
using RobloxFW.WorldEventSystem.Data;
using RobloxFW.WorldEventSystem.Data.Runtime;
using RobloxFW.WorldEventSystem.Interfaces;
using RobloxFW.WorldEventSystem.Strategies;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Manager
{
    public class WorldEventManager : ManagerBase, IUpdatable
    {
        [Header("References")]
        [SerializeField] private DissolveController dissolveController;
        
        public event Action<WorldEventEnum> OnWorldEventStarted;
        public event Action<WorldEventEnum, float> OnWorldEventTicking;
        public event Action<WorldEventEnum> OnWorldEventEnded;
        
        public List<IWorldEventStrategy> ActiveStrategies => _activeStrategies;
        
        
        private Dictionary<WorldEventEnum, IWorldEventStrategy> _worldEventStrategiesDic;
        private List<IWorldEventStrategy> _activeStrategies;
        
        // Temp
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Update()
        {
            OnUpdate(Time.deltaTime);
        }

        public override void Initialize()
        {
            _worldEventStrategiesDic = new Dictionary<WorldEventEnum, IWorldEventStrategy>()
            {
                {WorldEventEnum.SpeedBoost, new SpeedBoostEventStrategy()},
                {WorldEventEnum.LowGravity, new LowGravityEventStrategy()}
            };
            
            _activeStrategies = new List<IWorldEventStrategy>();
        }
        
        public void StartEvent(WorldEventContext context)
        {
            if (_worldEventStrategiesDic.TryGetValue(context.EventId, out var strategy))
            {
                if (!strategy.IsActive)
                {
                    strategy.Initialize(context);
                    strategy.OnEventStart();
                    
                    if (!_activeStrategies.Contains(strategy))
                        _activeStrategies.Add(strategy);
                    
                    OnWorldEventStarted?.Invoke(context.EventId);
                    
                    if (dissolveController != null && context.EnvironmentTextures != null && context.EnvironmentTextures.Length > 0)
                    {
                        dissolveController.TriggerMorph(context.EnvironmentTextures[0]);
                    }
                }
                else
                {
                    strategy.Initialize(context);
                }
            }
            else
            {
                GameLogger.LogWarning($"No strategy found for event: {context.EventId}");
            }
        }
        
        public void EndEvent(WorldEventEnum eventId)
        {
            if (_worldEventStrategiesDic.TryGetValue(eventId, out var strategy))
            {
                if (_activeStrategies.Contains(strategy))
                {
                    strategy.IsActive = false;
                    strategy.OnEventEnd();
                    _activeStrategies.Remove(strategy);
                    OnWorldEventEnded?.Invoke(eventId);
                }
            }
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = _activeStrategies.Count - 1; i >= 0; i--)
            {
                var strategy = _activeStrategies[i];
                
                if (strategy.IsActive)
                {
                    strategy.OnEventTick(deltaTime);
                    OnWorldEventTicking?.Invoke(strategy.EventId, deltaTime);
                }
            }
        }
    }
}

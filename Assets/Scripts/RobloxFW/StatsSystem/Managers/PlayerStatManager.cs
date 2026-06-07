using System;
using Dylanng.Core;
using Dylanng.Core.Base;
using RobloxFW.StatsSystem.Data;
using RobloxFW.StatsSystem.Events;
using UnityEngine;

namespace RobloxFW.StatsSystem.Systems
{
    public class PlayerStatManager : ManagerBase
    {
        [SerializeField] private EntityBaseStatsData entityStatsData;
        
        private float[] _baseValues;
        private float[] _modifiers;
        private float[] _cachedTotalValues;

        public override void Initialize()
        {
            ServiceLocator.Register<PlayerStatManager>(this);
            
            int statCount = Enum.GetNames(typeof(StatType)).Length;
            _baseValues = new float[statCount];
            _modifiers = new float[statCount];
            _cachedTotalValues = new float[statCount];

            LoadBaseStats();
        }

        private void LoadBaseStats()
        {
            if (entityStatsData != null && entityStatsData.InitialStats != null)
            {
                for (int i = 0; i < entityStatsData.InitialStats.Count; i++)
                {
                    StatConfig config = entityStatsData.InitialStats[i];
                    int index = (int)config.Type;
                    
                    _baseValues[index] = config.BaseValue;
                    _cachedTotalValues[index] = config.BaseValue;
                }
            }
        }
        
        public float GetStat(StatType type)
        {
            return _cachedTotalValues[(int)type];
        }
        
        public void AddBaseStat(StatType type, float amount)
        {
            int index = (int)type;
            float oldValue = _cachedTotalValues[index];
            
            _baseValues[index] += amount;
            
            UpdateTotalAndNotify(index, type, oldValue);
        }
        
        public void AddModifier(StatType type, float amount)
        {
            int index = (int)type;
            float oldValue = _cachedTotalValues[index];
            
            _modifiers[index] += amount;
            
            UpdateTotalAndNotify(index, type, oldValue);
        }
        
        public void RemoveModifier(StatType type, float amount)
        {
            int index = (int)type;
            float oldValue = _cachedTotalValues[index];
            
            _modifiers[index] -= amount;
            
            UpdateTotalAndNotify(index, type, oldValue);
        }
        
        private void UpdateTotalAndNotify(int index, StatType type, float oldValue)
        {
            float newValue = _baseValues[index] + _modifiers[index];
            _cachedTotalValues[index] = newValue;
            
            if (!Mathf.Approximately(oldValue, newValue))
            {
                EventBus.Publish(new StatChangedEvent(type, oldValue, newValue));
            }
        }
    }
}

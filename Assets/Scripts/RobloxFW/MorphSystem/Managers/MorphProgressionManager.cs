using Dylanng.Core;
using Dylanng.Core.Base;
using RobloxFW.MorphSystem.Data;
using RobloxFW.MorphSystem.Events;
using UnityEngine;

namespace RobloxFW.MorphSystem.Managers
{
    public class MorphProgressionManager : SystemBase
    {
        private float _currentWeight;
        private int _currentTierIndex;
        private MorphTierData _tierData;

        public override void Initialize()
        {
            ServiceLocator.Register<MorphProgressionManager>(this);
            
            _tierData = Resources.Load<MorphTierData>("Data/MorphTierData");
            if (_tierData == null)
            {
                GameLogger.LogError("[MorphProgressionManager] Không tìm thấy MorphTierData ở Resources/Data/MorphTierData!");
            }

            _currentWeight = 0f;
            _currentTierIndex = -1;
            
            if (_tierData != null && _tierData.Tiers != null && _tierData.Tiers.Count > 0)
            {
                CheckForNewTier();
            }

            EventBus.Subscribe<WeightGainedEvent>(OnWeightGained);
        }

        private void OnWeightGained(WeightGainedEvent evt)
        {
            _currentWeight += evt.WeightAdded;
            CheckForNewTier();
        }

        private void CheckForNewTier()
        {
            if (_tierData == null || _tierData.Tiers == null || _tierData.Tiers.Count == 0) return;

            int nextTierIndex = _currentTierIndex + 1;
            bool tierChanged = false;
            
            while (nextTierIndex < _tierData.Tiers.Count)
            {
                if (_currentWeight >= _tierData.Tiers[nextTierIndex].RequiredWeight)
                {
                    _currentTierIndex = nextTierIndex;
                    tierChanged = true;
                    nextTierIndex++;
                }
                else
                {
                    break;
                }
            }

            if (tierChanged)
            {
                MorphTier currentTier = _tierData.Tiers[_currentTierIndex];
                EventBus.Publish(new MorphTierChangedEvent(_currentTierIndex, currentTier, _currentWeight));
            }
        }
    }
}

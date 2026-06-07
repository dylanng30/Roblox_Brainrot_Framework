using Dylanng.Core;
using RobloxFW.MorphSystem.Data;

namespace RobloxFW.MorphSystem.Events
{
    public struct MorphTierChangedEvent : IEvent
    {
        public readonly int TierIndex;
        public readonly MorphTier NewTier;
        public readonly float CurrentWeight;

        public MorphTierChangedEvent(int tierIndex, MorphTier newTier, float currentWeight)
        {
            TierIndex = tierIndex;
            NewTier = newTier;
            CurrentWeight = currentWeight;
        }
    }
}

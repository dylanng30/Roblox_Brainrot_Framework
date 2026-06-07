using Dylanng.Core;

namespace RobloxFW.MorphSystem.Events
{
    public struct WeightGainedEvent : IEvent
    {
        public readonly float WeightAdded;

        public WeightGainedEvent(float weightAdded)
        {
            WeightAdded = weightAdded;
        }
    }
}

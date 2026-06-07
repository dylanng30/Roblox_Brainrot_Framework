using Dylanng.Core;
using RobloxFW.StatsSystem.Data;

namespace RobloxFW.StatsSystem.Events
{
    public struct StatChangedEvent : IEvent
    {
        public readonly StatType Type;
        public readonly float OldValue;
        public readonly float NewValue;

        public StatChangedEvent(StatType type, float oldValue, float newValue)
        {
            Type = type;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}

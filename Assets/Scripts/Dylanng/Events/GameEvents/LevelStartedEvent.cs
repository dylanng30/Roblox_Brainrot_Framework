using Dylanng.Core;

namespace Dylanng.Events.GameEvents
{
    public struct LevelStartedEvent : IEvent
    {
        public int LevelIndex;
    }
}

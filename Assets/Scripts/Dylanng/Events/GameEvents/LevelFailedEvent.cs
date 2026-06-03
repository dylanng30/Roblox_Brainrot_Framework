using Dylanng.Core;

namespace Dylanng.Events.GameEvents
{
    public struct LevelFailedEvent : IEvent
    {
        public int LevelIndex;
    }
}

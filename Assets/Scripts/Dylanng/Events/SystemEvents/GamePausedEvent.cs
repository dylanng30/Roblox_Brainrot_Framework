using Dylanng.Core;

namespace Dylanng.Events.SystemEvents
{
    public struct GamePausedEvent : IEvent
    {
        public bool IsPaused;
    }
}

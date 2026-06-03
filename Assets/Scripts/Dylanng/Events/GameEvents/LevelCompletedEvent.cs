using Dylanng.Core;

namespace Dylanng.Events.GameEvents
{
    public struct LevelCompletedEvent : IEvent
    {
        public int LevelIndex;
        public int Score;
    }
}

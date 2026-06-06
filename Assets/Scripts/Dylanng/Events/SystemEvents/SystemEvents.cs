using Dylanng.Core;

namespace Dylanng.Events.SystemEvents
{
    public struct GameBootedEvent : IEvent { }
    
    public struct GamePausedEvent : IEvent
    {
        public bool IsPaused;
    }
    
    public struct SceneLoadedEvent : IEvent
    {
        public string SceneName;
    }
}
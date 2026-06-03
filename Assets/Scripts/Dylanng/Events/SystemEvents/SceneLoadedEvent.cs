using Dylanng.Core;

namespace Dylanng.Events.SystemEvents
{
    public struct SceneLoadedEvent : IEvent
    {
        public string SceneName;
    }
}
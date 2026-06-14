using System.Collections.Generic;
using RobloxFW.WorldEventSystem.Data.Remote;

namespace RobloxFW.WorldEventSystem.Interfaces
{
    public interface IEventSelectionStrategy
    {
        RemoteWorldEventData GetNextEvent(List<RemoteWorldEventData> pool);
    }
}

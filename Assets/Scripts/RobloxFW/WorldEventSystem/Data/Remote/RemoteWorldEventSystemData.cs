using System;
using System.Collections.Generic;

namespace RobloxFW.WorldEventSystem.Data.Remote
{
    [Serializable]
    public class RemoteWorldEventSystemData
    {
        public ScheduleModeEnum ScheduleMode;
        public float EventCooldown = 60f;
        public bool WaitBeforeFirstEvent = true;
        public List<RemoteWorldEventData> EventPool = new List<RemoteWorldEventData>();
    }
}

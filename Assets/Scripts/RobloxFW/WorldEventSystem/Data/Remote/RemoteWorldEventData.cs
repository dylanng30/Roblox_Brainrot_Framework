using System;
using RobloxFW.WorldEventSystem.Data;

namespace RobloxFW.WorldEventSystem.Data.Remote
{
    [Serializable]
    public class RemoteWorldEventData
    {
        public WorldEventEnum EventId;
        public float Duration; // Seconds
        public float Multiplier = 1f;
        public int Weight = 1;
    }
}

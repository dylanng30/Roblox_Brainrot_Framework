using System;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Data.Runtime
{
    public struct WorldEventContext
    {
        public WorldEventEnum EventId;
        public float Duration;
        public float Multiplier;
        public Texture2D[] EnvironmentTextures;

        public WorldEventContext(WorldEventEnum eventId, float duration, float multiplier, Texture2D[] envTextures)
        {
            EventId = eventId;
            Duration = duration;
            Multiplier = multiplier;
            EnvironmentTextures = envTextures;
        }
    }
}

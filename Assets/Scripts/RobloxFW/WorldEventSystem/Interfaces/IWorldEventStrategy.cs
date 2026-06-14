using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RobloxFW.WorldEventSystem.Data;
using RobloxFW.WorldEventSystem.Data.Runtime;

namespace RobloxFW.WorldEventSystem.Interfaces
{
    public interface IWorldEventStrategy
    {
        WorldEventEnum EventId { get; }
        bool IsActive { get; set; }
        
        void Initialize(WorldEventContext context);
        void OnEventStart();
        void OnEventTick(float deltaTime);
        void OnEventEnd();
    }
}
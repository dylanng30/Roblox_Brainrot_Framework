using Dylanng.Core;
using UnityEngine;

namespace Dylanng.Events.InputEvents
{
    public struct SwipeEvent : IEvent
    {
        public Vector2 Direction;
    }
}
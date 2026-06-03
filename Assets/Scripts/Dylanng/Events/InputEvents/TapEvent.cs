using Dylanng.Core;
using UnityEngine;

namespace Dylanng.Events.InputEvents
{
    public struct TapEvent : IEvent
    {
        public Vector3 ScreenPosition;
    }
}
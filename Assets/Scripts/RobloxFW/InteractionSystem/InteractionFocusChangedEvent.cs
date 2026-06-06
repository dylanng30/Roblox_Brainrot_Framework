using System.Collections.Generic;
using Dylanng.Core;

namespace RobloxFW.InteractionSystem
{
    public struct InteractionFocusChangedEvent : IEvent
    {
        public IInteractable FocusedObject;
        public List<InteractionData> AvailableInteractions;
    }
}
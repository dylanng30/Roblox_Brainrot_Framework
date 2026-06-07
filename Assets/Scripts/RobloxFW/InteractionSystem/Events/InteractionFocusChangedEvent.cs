using System.Collections.Generic;
using Dylanng.Core;
using RobloxFW.InteractionSystem.Data;
using RobloxFW.InteractionSystem.Interfaces;

namespace RobloxFW.InteractionSystem
{
    public struct InteractionFocusChangedEvent : IEvent
    {
        public IInteractable FocusedObject;
        public List<InteractionData> AvailableInteractions;
    }
}
using Dylanng.Core;
using RobloxFW.EquipmentSystem.Data;
using RobloxFW.EquipmentSystem.Core;
using UnityEngine;

namespace RobloxFW.EquipmentSystem.Events
{
    public struct EquipmentChangedEvent : IEvent
    {
        public EquipmentSlot Slot;
        public EquipmentData NewEquipment;
        public EquipmentData OldEquipment;
        public GameObject VisualInstance;

        public EquipmentChangedEvent(EquipmentSlot slot, EquipmentData newEquipment, EquipmentData oldEquipment, GameObject visualInstance)
        {
            Slot = slot;
            NewEquipment = newEquipment;
            OldEquipment = oldEquipment;
            VisualInstance = visualInstance;
        }
    }
}

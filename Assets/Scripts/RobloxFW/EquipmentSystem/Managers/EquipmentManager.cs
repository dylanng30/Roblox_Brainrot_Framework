using System.Collections.Generic;
using UnityEngine;
using Dylanng.Core.Base;
using Dylanng.Core;
using RobloxFW.EquipmentSystem.Core;
using RobloxFW.EquipmentSystem.Data;
using RobloxFW.EquipmentSystem.Events;
using RobloxFW.EquipmentSystem.Components;
using RobloxFW.EquipmentSystem.Interfaces;

namespace RobloxFW.EquipmentSystem.Managers
{
    public class EquipmentManager : ManagerBase
    {
        private EquipmentSockets _sockets;
        private Dictionary<EquipmentSlot, IEquippable> _equippedItems;

        public override void Initialize()
        {
            ServiceLocator.Register<EquipmentManager>(this);
            _equippedItems = new Dictionary<EquipmentSlot, IEquippable>();
        }

        public void SetSockets(EquipmentSockets sockets)
        {
            _sockets = sockets;
        }

        public void Equip(EquipmentData data)
        {
            if (data == null) return;

            EquipmentSlot slot = data.Slot;
            EquipmentData oldData = null;

            if (_equippedItems.TryGetValue(slot, out IEquippable currentItem))
            {
                oldData = currentItem.EquipData;
                currentItem.Unequip();
                _equippedItems.Remove(slot);
            }

            IEquippable newItem = new EquippedItem(data);
            
            Transform socketTransform = _sockets != null ? _sockets.GetSocket(slot) : null;
            newItem.Equip(socketTransform);
            
            _equippedItems.Add(slot, newItem);

            EventBus.Publish<EquipmentChangedEvent>(new EquipmentChangedEvent(slot, data, oldData, newItem.VisualInstance));
        }

        public void Unequip(EquipmentSlot slot)
        {
            if (_equippedItems.TryGetValue(slot, out IEquippable currentItem))
            {
                EquipmentData oldData = currentItem.EquipData;
                currentItem.Unequip();
                _equippedItems.Remove(slot);

                EventBus.Publish<EquipmentChangedEvent>(new EquipmentChangedEvent(slot, null, oldData, null));
            }
        }
        
        public IEquippable GetEquippedItem(EquipmentSlot slot)
        {
            if (_equippedItems.TryGetValue(slot, out IEquippable item))
            {
                return item;
            }
            return null;
        }
    }
}

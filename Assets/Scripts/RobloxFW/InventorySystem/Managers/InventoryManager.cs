using System.Collections.Generic;
using UnityEngine;
using Dylanng.Core.Base;
using Dylanng.Core;
using RobloxFW.InventorySystem.Core;
using RobloxFW.InventorySystem.Data;
using RobloxFW.InventorySystem.Events;

namespace RobloxFW.InventorySystem.Managers
{
    public class InventoryManager : ManagerBase
    {
        [SerializeField] private int _maxSlots = 20;
        private List<InventorySlot> _slots;

        public override void Initialize()
        {
            ServiceLocator.Register<InventoryManager>(this);
            
            _slots = new List<InventorySlot>(_maxSlots);
            for (int i = 0; i < _maxSlots; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }

        public bool AddItem(ItemData data, int amount = 1)
        {
            if (data == null || amount <= 0) return false;

            int remainingAmount = amount;

            // 1. Try to stack with existing items
            foreach (var slot in _slots)
            {
                if (!slot.IsEmpty && slot.Item.Data.ItemID == data.ItemID)
                {
                    int spaceLeft = data.MaxStack - slot.Item.Quantity;
                    if (spaceLeft > 0)
                    {
                        int amountToAdd = Mathf.Min(remainingAmount, spaceLeft);
                        slot.Item.AddQuantity(amountToAdd);
                        remainingAmount -= amountToAdd;
                        
                        EventBus.Publish<InventoryChangedEvent>(new InventoryChangedEvent(data, slot.Item.Quantity, amountToAdd));

                        if (remainingAmount <= 0) return true;
                    }
                }
            }

            // 2. Try to find empty slots for the remaining amount
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty)
                {
                    int amountToAdd = Mathf.Min(remainingAmount, data.MaxStack);
                    slot.SetItem(new InventoryItem(data, amountToAdd));
                    remainingAmount -= amountToAdd;

                    EventBus.Publish<InventoryChangedEvent>(new InventoryChangedEvent(data, slot.Item.Quantity, amountToAdd));

                    if (remainingAmount <= 0) return true;
                }
            }

            return false;
        }

        public bool RemoveItem(ItemData data, int amount = 1)
        {
            if (data == null || amount <= 0) return false;

            int amountToRemove = amount;

            if (GetItemQuantity(data) < amount) return false;

            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                var slot = _slots[i];
                if (!slot.IsEmpty && slot.Item.Data.ItemID == data.ItemID)
                {
                    int removed = Mathf.Min(amountToRemove, slot.Item.Quantity);
                    slot.Item.RemoveQuantity(removed);
                    amountToRemove -= removed;

                    EventBus.Publish(new InventoryChangedEvent(data, slot.Item.Quantity, -removed));

                    if (slot.Item.Quantity <= 0)
                    {
                        slot.Clear();
                    }

                    if (amountToRemove <= 0) return true;
                }
            }

            return false;
        }

        public int GetItemQuantity(ItemData data)
        {
            if (data == null) return 0;

            int total = 0;
            foreach (var slot in _slots)
            {
                if (!slot.IsEmpty && slot.Item.Data.ItemID == data.ItemID)
                {
                    total += slot.Item.Quantity;
                }
            }
            return total;
        }

        public bool HasItem(ItemData data, int amount = 1)
        {
            return GetItemQuantity(data) >= amount;
        }
    }
}

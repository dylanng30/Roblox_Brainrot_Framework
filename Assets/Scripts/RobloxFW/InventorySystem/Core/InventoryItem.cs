using RobloxFW.InventorySystem.Interfaces;
using RobloxFW.InventorySystem.Data;
using UnityEngine;

namespace RobloxFW.InventorySystem.Core
{
    public class InventoryItem : IItem
    {
        public ItemData Data { get; private set; }
        public int Quantity { get; private set; }

        public InventoryItem(ItemData data, int quantity)
        {
            Data = data;
            Quantity = quantity;
        }

        public void AddQuantity(int amount)
        {
            Quantity += amount;
            if (Quantity > Data.MaxStack) Quantity = Data.MaxStack;
        }

        public void RemoveQuantity(int amount)
        {
            Quantity -= amount;
            if (Quantity < 0) Quantity = 0;
        }

        public void Use(GameObject user)
        {
            if (Data != null && Data.UseStrategy != null)
            {
                Data.UseStrategy.Use(user, 1);
            }
        }
    }
}

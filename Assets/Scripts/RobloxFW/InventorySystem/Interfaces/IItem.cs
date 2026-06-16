using RobloxFW.InventorySystem.Data;

namespace RobloxFW.InventorySystem.Interfaces
{
    public interface IItem
    {
        ItemData Data { get; }
        int Quantity { get; }

        void AddQuantity(int amount);
        void RemoveQuantity(int amount);
        void Use(UnityEngine.GameObject user);
    }
}

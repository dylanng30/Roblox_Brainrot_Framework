using UnityEngine;

namespace RobloxFW.InventorySystem.Interfaces
{
    public interface IItemUseStrategy
    {
        void Use(GameObject user, int amount = 1);
    }
}

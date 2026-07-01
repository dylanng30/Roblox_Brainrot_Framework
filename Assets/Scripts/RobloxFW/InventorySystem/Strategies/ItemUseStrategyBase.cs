using UnityEngine;
using RobloxFW.InventorySystem.Interfaces;

namespace RobloxFW.InventorySystem.Strategies
{
    public abstract class ItemUseStrategyBase : ScriptableObject, IItemUseStrategy
    {
        public abstract void Use(GameObject user, int amount = 1);
    }
}

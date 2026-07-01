using UnityEngine;
using Dylanng.Core.Base;
using RobloxFW.InventorySystem.Strategies;

namespace RobloxFW.InventorySystem.Data
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "RobloxFW/Items/Item")]
    public class ItemData : ScriptableData
    {
        public string ItemID;
        public string ItemName;
        [TextArea] public string Description;
        public Sprite Icon;
        public int MaxStack = 99;
        public bool IsConsumable;

        [Tooltip("Strategy used when this item is consumed/used.")]
        public ItemUseStrategyBase UseStrategy;
    }
}

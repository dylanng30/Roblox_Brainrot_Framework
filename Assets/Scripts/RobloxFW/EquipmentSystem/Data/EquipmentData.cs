using UnityEngine;
using RobloxFW.InventorySystem.Data;
using RobloxFW.EquipmentSystem.Core;

namespace RobloxFW.EquipmentSystem.Data
{
    [CreateAssetMenu(fileName = "New Equipment Data", menuName = "RobloxFW/Equipment/Equipment")]
    public class EquipmentData : ItemData
    {
        public EquipmentSlot Slot;
        public GameObject VisualPrefab;
        // Specific stats and other properties can be added here
    }
}

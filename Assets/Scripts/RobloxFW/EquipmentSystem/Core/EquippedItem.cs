using UnityEngine;
using RobloxFW.EquipmentSystem.Interfaces;
using RobloxFW.EquipmentSystem.Data;

namespace RobloxFW.EquipmentSystem.Core
{
    public class EquippedItem : IEquippable
    {
        public EquipmentData EquipData { get; private set; }
        public GameObject VisualInstance { get; private set; }

        public EquippedItem(EquipmentData data)
        {
            EquipData = data;
        }

        public void Equip(Transform socket)
        {
            if (EquipData.VisualPrefab != null && socket != null)
            {
                // In a real project, we would use a PoolManager.
                // Assuming PoolManager exists in ServiceLocator.Get<PoolManager>()
                // For this example, simple Instantiate to keep it understandable.
                VisualInstance = GameObject.Instantiate(EquipData.VisualPrefab, socket);
                VisualInstance.transform.localPosition = Vector3.zero;
                VisualInstance.transform.localRotation = Quaternion.identity;
            }
        }

        public void Unequip()
        {
            if (VisualInstance != null)
            {
                GameObject.Destroy(VisualInstance);
                VisualInstance = null;
            }
        }

        public void TriggerAction(GameObject user)
        {
            if (EquipData != null && EquipData.UseStrategy != null)
            {
                EquipData.UseStrategy.Use(user, 1);
            }
        }
    }
}

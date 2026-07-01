using UnityEngine;
using RobloxFW.EquipmentSystem.Data;

namespace RobloxFW.EquipmentSystem.Interfaces
{
    public interface IEquippable
    {
        EquipmentData EquipData { get; }
        GameObject VisualInstance { get; }

        void Equip(Transform socket);
        void Unequip();
        void TriggerAction(GameObject user);
    }
}

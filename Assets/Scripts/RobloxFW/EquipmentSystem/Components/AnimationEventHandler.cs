using UnityEngine;
using RobloxFW.EquipmentSystem.Core;
using RobloxFW.EquipmentSystem.Managers;
using Dylanng.Core;

namespace RobloxFW.EquipmentSystem.Components
{
    public class AnimationEventHandler : MonoBehaviour
    {
        // This method should be called by Animation Events set up in the Unity Animator window.
        // For example, on a "Hit" frame of an attack animation.
        public void OnAttackHitEvent(int slotIndex)
        {
            EquipmentSlot slot = (EquipmentSlot)slotIndex;
            var equipmentManager = ServiceLocator.Get<EquipmentManager>();
            
            if (equipmentManager != null)
            {
                var equippedItem = equipmentManager.GetEquippedItem(slot);
                if (equippedItem != null)
                {
                    // Pass the root game object (typically the player) as the user
                    equippedItem.TriggerAction(transform.root.gameObject);
                }
            }
        }
    }
}

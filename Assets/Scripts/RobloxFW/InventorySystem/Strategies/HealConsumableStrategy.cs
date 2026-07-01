using UnityEngine;

namespace RobloxFW.InventorySystem.Strategies
{
    [CreateAssetMenu(fileName = "New Heal Strategy", menuName = "RobloxFW/Items/Strategies/Heal Consumable")]
    public class HealConsumableStrategy : ItemUseStrategyBase
    {
        public int HealAmount = 10;

        public override void Use(GameObject user, int amount = 1)
        {
            // Example implementation
            Debug.Log($"[HealConsumableStrategy] Healed {user.name} for {HealAmount * amount} HP.");
            
            // In a real scenario, we might call an event or a stat manager on the user:
            // var statManager = user.GetComponent<PlayerStatManager>();
            // if (statManager != null) statManager.Heal(HealAmount * amount);
        }
    }
}

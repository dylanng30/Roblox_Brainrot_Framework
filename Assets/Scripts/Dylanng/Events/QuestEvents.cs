using Dylanng.Core;

namespace Dylanng.Events
{
    public struct EnemyKilledEvent : IEvent
    {
        public string EnemyID;

        public EnemyKilledEvent(string enemyID)
        {
            EnemyID = enemyID;
        }
    }

    public struct ItemLootedEvent : IEvent
    {
        public string ItemID;
        public int Quantity;

        public ItemLootedEvent(string itemID, int quantity)
        {
            ItemID = itemID;
            Quantity = quantity;
        }
    }
}

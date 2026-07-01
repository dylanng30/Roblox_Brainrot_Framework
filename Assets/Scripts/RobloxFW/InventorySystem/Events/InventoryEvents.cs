using Dylanng.Core;
using RobloxFW.InventorySystem.Data;

namespace RobloxFW.InventorySystem.Events
{
    public struct InventoryChangedEvent : IEvent
    {
        public ItemData Item;
        public int NewQuantity;
        public int Delta;

        public InventoryChangedEvent(ItemData item, int newQuantity, int delta)
        {
            Item = item;
            NewQuantity = newQuantity;
            Delta = delta;
        }
    }
}

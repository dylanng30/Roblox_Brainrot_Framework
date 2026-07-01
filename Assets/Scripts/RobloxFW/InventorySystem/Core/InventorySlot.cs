using RobloxFW.InventorySystem.Interfaces;

namespace RobloxFW.InventorySystem.Core
{
    public class InventorySlot
    {
        public IItem Item { get; private set; }
        public bool IsEmpty => Item == null || Item.Quantity <= 0;

        public void SetItem(IItem item)
        {
            Item = item;
        }

        public void Clear()
        {
            Item = null;
        }
    }
}

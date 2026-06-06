using Dylanng.Core;

namespace RobloxFW.EconomySystem
{
    public struct CurrencyChangedEvent : IEvent
    {
        public CurrencyType CurrencyType;
        public int AmountChanged;
        public int NewBalance;
        
        public CurrencyChangedEvent(CurrencyType type, int amountChanged, int newBalance)
        {
            CurrencyType = type;
            AmountChanged = amountChanged;
            NewBalance = newBalance;
        }
    }
}

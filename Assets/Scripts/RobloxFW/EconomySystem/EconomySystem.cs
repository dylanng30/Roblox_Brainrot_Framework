using System;
using Dylanng.Core;
using Dylanng.Core.Base;
using Dylanng.Core.Systems.TickSystem;
using Dylanng.Managers;

namespace RobloxFW.EconomySystem
{
    public class EconomySystem : SystemBase, IOneSecondTickable
    {
        private SaveLoadManager SaveManager => ServiceLocator.Get<SaveLoadManager>();
        
        private bool _isChanged = false;

        public override void Initialize()
        {
            ServiceLocator.Register<EconomySystem>(this);
            
            ITickSystem tickSystem = ServiceLocator.Get<ITickSystem>();
            tickSystem?.Register(this);
        }

        public int GetBalance(CurrencyType type)
        {
            if (SaveManager?.CurrentData == null)
            {
                return 0;
            }

            return type switch
            {
                CurrencyType.Coin => SaveManager.CurrentData.Coins,
                CurrencyType.Gem => SaveManager.CurrentData.Gems,
                _ => 0
            };
        }

        public bool HasEnough(CurrencyType type, int amount)
        {
            if (amount < 0) return false;
            return GetBalance(type) >= amount;
        }

        public void AddCurrency(CurrencyType type, int amount)
        {
            if (amount <= 0) return;

            UpdateBalance(type, amount);
            
            _isChanged = true;
        }

        public void SpendCurrency(CurrencyType type, int amount)
        {
            if (amount <= 0) return;

            if (!HasEnough(type, amount))
            {
                GameLogger.LogWarning($"[EconomySystem] Not enough {type} to spend {amount}.");
                return;
            }

            UpdateBalance(type, -amount);
            
            SaveManager?.Save();
            _isChanged = false;
        }

        private void UpdateBalance(CurrencyType type, int changeAmount)
        {
            if (SaveManager?.CurrentData == null) return;

            int newBalance = 0;
            
            switch (type)
            {
                case CurrencyType.Coin:
                    SaveManager.CurrentData.Coins += changeAmount;
                    newBalance = SaveManager.CurrentData.Coins;
                    break;
                case CurrencyType.Gem:
                    SaveManager.CurrentData.Gems += changeAmount;
                    newBalance = SaveManager.CurrentData.Gems;
                    break;
            }
            
            EventBus.Publish(new CurrencyChangedEvent(type, changeAmount, newBalance));
        }
        
        public void OnOneSecondTick()
        {
            if (_isChanged && SaveManager != null)
            {
                SaveManager.Save();
                _isChanged = false;
            }
        }
    }
}

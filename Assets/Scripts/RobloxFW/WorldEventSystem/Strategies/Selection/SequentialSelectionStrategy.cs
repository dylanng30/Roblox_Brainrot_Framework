using System.Collections.Generic;
using UnityEngine;
using RobloxFW.WorldEventSystem.Data.Remote;
using RobloxFW.WorldEventSystem.Interfaces;

namespace RobloxFW.WorldEventSystem.Strategies.Selection
{
    public class SequentialSelectionStrategy : IEventSelectionStrategy
    {
        private int _currentIndex = 0;

        public RemoteWorldEventData GetNextEvent(List<RemoteWorldEventData> pool)
        {
            if (pool == null || pool.Count == 0)
            {
                Debug.LogWarning("Event Pool is empty");
                return null;
            }

            if (_currentIndex >= pool.Count)
            {
                _currentIndex = 0;
            }

            var item = pool[_currentIndex];
            _currentIndex++;

            return item;
        }
    }
}

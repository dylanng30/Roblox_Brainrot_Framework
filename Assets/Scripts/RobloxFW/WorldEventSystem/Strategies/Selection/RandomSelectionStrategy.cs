using System.Collections.Generic;
using UnityEngine;
using RobloxFW.WorldEventSystem.Data.Remote;
using RobloxFW.WorldEventSystem.Interfaces;

namespace RobloxFW.WorldEventSystem.Strategies.Selection
{
    public class RandomSelectionStrategy : IEventSelectionStrategy
    {
        public RemoteWorldEventData GetNextEvent(List<RemoteWorldEventData> pool)
        {
            if (pool == null || pool.Count == 0)
            {
                Debug.LogWarning("Event Pool is empty. Returning default context.");
                return null;
            }

            int totalWeight = 0;
            foreach (var item in pool)
            {
                totalWeight += item.Weight;
            }

            int randomValue = Random.Range(0, totalWeight);
            int currentSum = 0;

            foreach (var item in pool)
            {
                currentSum += item.Weight;
                if (randomValue < currentSum)
                {
                    return item;
                }
            }
            
            return pool[0];
        }
    }
}

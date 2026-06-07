using System;
using System.Collections.Generic;
using UnityEngine;
using Dylanng.Core.Base;

namespace RobloxFW.StatsSystem.Data
{
    [Serializable]
    public struct StatConfig
    {
        public StatType Type;
        public float BaseValue;
    }

    [CreateAssetMenu(fileName = "EntityBaseStatsData", menuName = "RobloxFW/StatsSystem/EntityBaseStatsData")]
    public class EntityBaseStatsData : ScriptableData
    {
        public List<StatConfig> InitialStats = new List<StatConfig>();
    }
}

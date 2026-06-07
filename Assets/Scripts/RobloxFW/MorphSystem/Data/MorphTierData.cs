using System.Collections.Generic;
using UnityEngine;
using Dylanng.Core.Base;

namespace RobloxFW.MorphSystem.Data
{
    [CreateAssetMenu(fileName = "MorphTierData", menuName = "RobloxFW/MorphSystem/MorphTierData")]
    public class MorphTierData : ScriptableData
    {
        [Tooltip("Danh sách các mốc biến hình, YÊU CẦU phải được sắp xếp theo RequiredWeight tăng dần.")]
        public List<MorphTier> Tiers = new List<MorphTier>();
    }
}

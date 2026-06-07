using System;
using UnityEngine;

namespace RobloxFW.MorphSystem.Data
{
    [Serializable]
    public struct MorphTier
    {
        [Tooltip("Số cân nặng cần thiết để tiến hóa lên mốc này")]
        public float RequiredWeight;
        
        [Tooltip("Tham chiếu đến Model 3D của nhân vật cho mốc này")]
        public GameObject ModelPrefab;
        
        [Tooltip("Hệ số scale cho model")]
        public float ScaleMultiplier;
        
        [Tooltip("Tốc độ di chuyển hoặc hệ số tốc độ")]
        public float MovementSpeed;
    }
}

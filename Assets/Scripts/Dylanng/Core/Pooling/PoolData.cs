using Dylanng.Core.Base;
using UnityEngine;

namespace Dylanng.Core.Pooling
{
    [CreateAssetMenu(fileName = "NewPoolData", menuName = "PoolSO/PoolData")]
    public class PoolData : ScriptableData
    {
        public string PoolKey;
        public PoolableObject Prefab;
    }
}

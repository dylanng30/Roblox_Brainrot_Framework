using UnityEngine;

namespace Dylanng
{
    public abstract class PoolableObject : EntityBase, IPoolable
    {
        [field: SerializeField] public string PoolKey { get; set; }
        public virtual void OnSpawn() => SetActive(true);
        public virtual void OnDespawn() => SetActive(false);
    }
}

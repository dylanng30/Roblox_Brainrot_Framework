
using Dylanng.Core.Base;

namespace Dylanng.Core.Pooling
{
    public abstract class PoolableObject : MonoBehaviourBase, IPoolable
    {
        public virtual void OnSpawn() => SetActive(true);
        public virtual void OnDespawn() => SetActive(false);
    }
}

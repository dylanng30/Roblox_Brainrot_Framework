using Dylanng.Core;
using UnityEngine;

namespace Dylanng.Events
{
    public struct EntityDamagedEvent : IEvent
    {
        public GameObject Target;
        public GameObject Source;
        public float DamageAmount;
        public bool IsCritical;

        public EntityDamagedEvent(GameObject target, GameObject source, float damageAmount, bool isCritical)
        {
            Target = target;
            Source = source;
            DamageAmount = damageAmount;
            IsCritical = isCritical;
        }
    }

    public struct EntityDiedEvent : IEvent
    {
        public GameObject Entity;

        public EntityDiedEvent(GameObject entity)
        {
            Entity = entity;
        }
    }
}

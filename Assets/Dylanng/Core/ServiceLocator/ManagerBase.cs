using UnityEngine;

namespace Dylanng
{
    public abstract class ManagerBase : MonoBehaviourBase, IManager
    {
        public bool IsInitialized { get; private set; }
        
        public virtual void Initialize()
        {
            IsInitialized = true;
            EventBus.Subscribe<IGameBootedEvent>(OnGameBooted);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventBus.Unsubscribe<IGameBootedEvent>(OnGameBooted);
        }

        protected virtual void OnGameBooted(IGameBootedEvent evt)
        {
            
        }
    }
}
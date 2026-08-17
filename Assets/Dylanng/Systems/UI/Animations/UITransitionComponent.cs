using System;

namespace Dylanng
{
    public abstract class UITransitionComponent : MonoBehaviourBase, IUITransition
    {
        public virtual void SetBeforeOpenState() { }
        public virtual void SetBeforeCloseState() { }
        public abstract void PlayOpen(Action onComplete);
        public abstract void PlayClose(Action onComplete);
    }
}

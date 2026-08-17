using System;

namespace Dylanng
{
    public interface IUITransition
    {
        void SetBeforeOpenState();
        void SetBeforeCloseState();
        void PlayOpen(Action onComplete);
        void PlayClose(Action onComplete);
    }
}

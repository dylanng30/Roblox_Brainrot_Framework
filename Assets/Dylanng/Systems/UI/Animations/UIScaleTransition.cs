using System;
using UnityEngine;
using DG.Tweening;

namespace Dylanng
{
    [Serializable]
    public class TweenConfig
    {
        public float duration = 0.5f;
        public Ease ease = Ease.OutBack;
        public float delay = 0f;
    }

    public class UIScaleTransition : UITransitionComponent
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private Vector3 openScale = Vector3.one;
        [SerializeField] private Vector3 closedScale = Vector3.zero;

        [Header("Open Config")]
        [SerializeField] private TweenConfig openConfig = new TweenConfig { ease = Ease.OutBack };

        [Header("Close Config")]
        [SerializeField] private TweenConfig closeConfig = new TweenConfig { ease = Ease.InBack };

        private Action _onCompleteAction;
        private TweenCallback _cachedTweenCallback;
        private TweenCallback CachedTweenCallback => _cachedTweenCallback ??= OnTweenComplete;

        public override void SetBeforeOpenState()
        {
            if (target != null) target.localScale = closedScale;
        }

        public override void SetBeforeCloseState()
        {
            if (target != null) target.localScale = openScale;
        }

        public override void PlayOpen(Action onComplete)
        {
            _onCompleteAction = onComplete;
            if (target == null)
            {
                OnTweenComplete();
                return;
            }

            target.DOScale(openScale, openConfig.duration)
                  .SetEase(openConfig.ease)
                  .SetDelay(openConfig.delay)
                  .OnComplete(CachedTweenCallback);
        }

        public override void PlayClose(Action onComplete)
        {
            _onCompleteAction = onComplete;
            if (target == null)
            {
                OnTweenComplete();
                return;
            }

            target.DOScale(closedScale, closeConfig.duration)
                  .SetEase(closeConfig.ease)
                  .SetDelay(closeConfig.delay)
                  .OnComplete(CachedTweenCallback);
        }

        private void OnTweenComplete()
        {
            _onCompleteAction?.Invoke();
        }
    }
}

using System;
using UnityEngine;
using DG.Tweening;

namespace Dylanng
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFadeTransition : UITransitionComponent
    {
        [SerializeField] private CanvasGroup target;
        [SerializeField] private float openAlpha = 1f;
        [SerializeField] private float closedAlpha = 0f;

        [Header("Open Config")]
        [SerializeField] private TweenConfig openConfig = new TweenConfig { duration = 0.3f, ease = Ease.Linear };

        [Header("Close Config")]
        [SerializeField] private TweenConfig closeConfig = new TweenConfig { duration = 0.3f, ease = Ease.Linear };

        private Action _onCompleteAction;
        private TweenCallback _cachedTweenCallback;
        private TweenCallback CachedTweenCallback => _cachedTweenCallback ??= OnTweenComplete;

        private void Reset()
        {
            if (target == null) target = GetComponent<CanvasGroup>();
        }

        public override void SetBeforeOpenState()
        {
            if (target != null) target.alpha = closedAlpha;
        }

        public override void SetBeforeCloseState()
        {
            if (target != null) target.alpha = openAlpha;
        }

        public override void PlayOpen(Action onComplete)
        {
            _onCompleteAction = onComplete;
            if (target == null)
            {
                OnTweenComplete();
                return;
            }

            target.DOFade(openAlpha, openConfig.duration)
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

            target.DOFade(closedAlpha, closeConfig.duration)
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

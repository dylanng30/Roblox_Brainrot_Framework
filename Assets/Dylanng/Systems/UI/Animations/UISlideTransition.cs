using System;
using UnityEngine;
using DG.Tweening;

namespace Dylanng
{
    public class UISlideTransition : UITransitionComponent
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private Vector2 openPosition = Vector2.zero;
        [SerializeField] private Vector2 closedPosition = new Vector2(0, -1000f); // Default slide from bottom

        [Header("Open Config")]
        [SerializeField] private TweenConfig openConfig = new TweenConfig { duration = 0.4f, ease = Ease.OutCubic };

        [Header("Close Config")]
        [SerializeField] private TweenConfig closeConfig = new TweenConfig { duration = 0.3f, ease = Ease.InCubic };

        private Action _onCompleteAction;
        private TweenCallback _cachedTweenCallback;
        private TweenCallback CachedTweenCallback => _cachedTweenCallback ??= OnTweenComplete;

        public override void SetBeforeOpenState()
        {
            if (target != null) target.anchoredPosition = closedPosition;
        }

        public override void SetBeforeCloseState()
        {
            if (target != null) target.anchoredPosition = openPosition;
        }

        public override void PlayOpen(Action onComplete)
        {
            _onCompleteAction = onComplete;
            if (target == null)
            {
                OnTweenComplete();
                return;
            }

            target.DOAnchorPos(openPosition, openConfig.duration)
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

            target.DOAnchorPos(closedPosition, closeConfig.duration)
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

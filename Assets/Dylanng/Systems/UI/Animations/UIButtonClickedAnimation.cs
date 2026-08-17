using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dylanng
{
    public class UIButtonClickedAnimation : UIAnimationBase, IPointerDownHandler, IPointerUpHandler
    {
        [Header("--- CLICK SETTINGS ---")]
        [SerializeField] private float pressScale = 0.9f;
        [SerializeField] private float animationDuration = 0.15f;

        private Vector3 _originalScale;
        private bool _isInitialized;

        protected override void Awake()
        {
            base.Awake();
            InitializeScale();
        }

        private void InitializeScale()
        {
            if (!_isInitialized)
            {
                _originalScale = transform.localScale;
                _isInitialized = true;
            }
        }

        public override void ResetToInitialState()
        {
            InitializeScale();
            transform.localScale = _originalScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            KillCurrentTween();

            _currentTween = transform.DOScale(_originalScale * pressScale, animationDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            KillCurrentTween();

            _currentTween = transform.DOScale(_originalScale, animationDuration * 1.5f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }
    }
}
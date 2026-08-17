using DG.Tweening;
using UnityEngine;

namespace Dylanng
{
    public class UIIdleBounceAnimation : UIAnimationBase
    {
        [Header("--- BOUNCE SETTINGS ---")]
        [SerializeField] private float bounceScaleMultipler = 1.05f;
        [SerializeField] private float cycleDuration = 0.8f;
        [SerializeField] private Ease easeType = Ease.InOutSine;

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

        protected override void OnEnable()
        {
            base.OnEnable();
            StartBouncing();
        }

        public override void ResetToInitialState()
        {
            InitializeScale();
            transform.localScale = _originalScale;
        }

        private void StartBouncing()
        {
            KillCurrentTween();
            _currentTween = transform.DOScale(_originalScale * bounceScaleMultipler, cycleDuration)
                .SetEase(easeType)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }
    }
}
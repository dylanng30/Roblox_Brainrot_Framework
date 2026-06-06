using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dylanng.Core.UI.UIAnimations
{
    public class UiAnimationButtonClick: UIAnimationBase, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Animation Settings")]
        [SerializeField] private float pressScale = 0.9f;
        [SerializeField] private float animationDuration = 0.15f;
        
        // [Header("VFX")]
        // [SerializeField] private ParticleSystem _clickParticles;

        private Vector3 _originalScale;
        private bool _isInitialized;

        private void Awake()
        {
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

            // if (_clickParticles != null)
            // {
            //     _clickParticles.Play();
            // }
        }
    }
}
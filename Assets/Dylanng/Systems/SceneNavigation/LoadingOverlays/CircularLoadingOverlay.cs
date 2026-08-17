using System.Collections;
using UnityEngine;

namespace Dylanng
{
    public class CircularLoadingOverlay : BaseLoadingOverlay
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform loadingIcon;
        [SerializeField] private float rotationSpeed = 200f;
        [SerializeField] private float fadeTime = 0.5f;

        private bool _isRotating = false;

        private void OnValidate()
        {
            if (canvasGroup == null) canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        protected override void Awake()
        {
            base.Awake();
            if (canvasGroup == null) canvasGroup = GetComponentInParent<CanvasGroup>();
            if (canvasGroup == null) GameLogger.LogError("[CircularLoadingOverlay] Canvas Group is null");
        }

        private void Update()
        {
            if (_isRotating && loadingIcon != null)
            {
                loadingIcon.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
            }
        }

        public override IEnumerator StartLoad()
        {
            _isRotating = true;
            yield return FadeTo(1f, fadeTime);
        }

        public override IEnumerator EndLoad()
        {
            yield return FadeTo(0f, fadeTime);
            _isRotating = false;
        }

        private IEnumerator FadeTo(float targetAlpha, float duration)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}
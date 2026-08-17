using System;
using System.Collections;
using UnityEngine;

namespace Dylanng
{
    public class FadeLoadingOverlay : BaseLoadingOverlay
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeOutTime = 1f;

        private void OnValidate()
        {
            if (canvasGroup == null) canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        protected override void Awake()
        {
            base.Awake();
            if (canvasGroup == null) canvasGroup = GetComponentInParent<CanvasGroup>();
            if (canvasGroup == null) GameLogger.LogError("[FadeLoadingOverlay] Canvas Group is null");
        }

        public override IEnumerator StartLoad()
        {
            // Fade In
            yield return FadeTo(1f, fadeInTime);
        }

        public override IEnumerator EndLoad()
        {
            // Fade Out
            yield return FadeTo(0f, fadeInTime);
        }

        private IEnumerator FadeTo(float targetAlpha, float duration)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }
            canvasGroup.alpha = targetAlpha;
        }
    }
}
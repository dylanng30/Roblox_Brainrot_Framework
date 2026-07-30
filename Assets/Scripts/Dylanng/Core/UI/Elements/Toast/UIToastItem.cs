using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Dylanng.Core.UI.Elements.Toast
{
    public class UIToastItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private float moveDistance = 50f;

        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowToast(string message, float duration, Action onComplete)
        {
            messageText.text = message;
            
            // Reset state
            canvasGroup.alpha = 0f;
            Vector2 anchoredPos = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = new Vector2(anchoredPos.x, anchoredPos.y - moveDistance);

            Sequence seq = DOTween.Sequence();
            seq.Append(canvasGroup.DOFade(1f, fadeDuration));
            seq.Join(_rectTransform.DOAnchorPosY(anchoredPos.y, fadeDuration).SetEase(Ease.OutBack));
            seq.AppendInterval(duration);
            seq.Append(canvasGroup.DOFade(0f, fadeDuration));
            seq.Join(_rectTransform.DOAnchorPosY(anchoredPos.y + moveDistance, fadeDuration).SetEase(Ease.InBack));
            
            seq.OnComplete(() =>
            {
                // Reset position for pool
                _rectTransform.anchoredPosition = anchoredPos;
                onComplete?.Invoke();
            });
        }
    }
}

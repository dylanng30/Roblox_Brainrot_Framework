using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dylanng.Core.UI.Elements.Components
{
    public class UIProgressBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private float fillDuration = 0.5f;
        [SerializeField] private Ease fillEase = Ease.OutQuad;

        private float _currentValue;

        public void SetProgress(float value, bool immediate = false)
        {
            value = Mathf.Clamp01(value);
            _currentValue = value;

            if (fillImage != null)
            {
                if (immediate)
                {
                    fillImage.DOKill();
                    fillImage.fillAmount = value;
                }
                else
                {
                    fillImage.DOKill();
                    fillImage.DOFillAmount(value, fillDuration).SetEase(fillEase);
                }
            }
        }
        
        public float GetProgress() => _currentValue;
    }
}

using DG.Tweening;
using Dylanng.Core.Base;
using Dylanng.Core.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Dylanng.Core.UI
{
    public abstract class UIBase : MonoBehaviourBase
    {
        public bool IsOpen { get; private set; }
        public virtual void Initialize() { }
        public virtual void Setup(object data = null) { }
        public virtual void Show()
        {
            IsOpen = true;
            gameObject.SetActive(true);
            OnShow();
        }
        public virtual void Hide()
        {
            IsOpen = false;
            OnHide();
            gameObject.SetActive(false);
        }
        
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }

    public abstract class UIScreen : UIBase { }
    public abstract class UIPopup : UIBase 
    {
        [Header("Popup settings")]
        [SerializeField] protected bool isExecutingPopupAnimation = true;
        [SerializeField] protected float popupDuration = 0.5f;
        [SerializeField] protected Ease popupEase = Ease.OutBack;

        [Space(5)]
        [SerializeField] protected bool isExecutingPopdownAnimation = true;
        [SerializeField] protected float popdownDuration = 0.5f;
        [SerializeField] protected Ease popdownEase = Ease.InBack;


        [Header("Popup References")]
        [SerializeField] protected Button closeButton;
        [SerializeField] protected RectTransform mainPanel;

        public override void Initialize()
        {
            base.Initialize();
            
            if (closeButton) closeButton.onClick.AddListener(Close);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (closeButton) closeButton.onClick.RemoveAllListeners();
        }

        public override void Show()
        {
            base.Show();

            // Do popup animation
            if (isExecutingPopupAnimation)
            {
                mainPanel.localScale = Vector3.one / 2f;
                mainPanel.DOScale(Vector3.one, 0.5f).SetEase(popupEase);
            }
            else
            {
                mainPanel.localScale = Vector3.one;
            }
        }

        public override void Hide()
        {
            // Do popdown animation
            mainPanel.localScale = Vector3.one;

            if (isExecutingPopdownAnimation)
            {
                mainPanel.DOScale(Vector3.zero, 0.5f)
                    .SetEase(popdownEase)
                    .OnComplete(() =>
                    {
                        base.Hide();
                    });
            }
            else
            {
                base.Hide();
            }
        }

        public virtual void Close()
        {
            ServiceLocator.Get<UIManager>().CloseCurrentPopup();
        }
    }
}

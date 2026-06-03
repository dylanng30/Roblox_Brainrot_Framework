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
        [Header("Popup Settings")]
        [SerializeField] protected bool _closeOnBackdropClick = true;
        [SerializeField] protected Button _closeButton;

        public override void Initialize()
        {
            base.Initialize();
            
            if (_closeButton)
            {
                _closeButton.onClick.AddListener(Close);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_closeButton)
            {
                _closeButton.onClick.RemoveAllListeners();
            }
        }

        public virtual void Close()
        {
            ServiceLocator.Get<UIManager>().CloseCurrentPopup();
        }
    }
}

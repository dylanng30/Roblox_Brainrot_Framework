using UnityEngine;
using UnityEngine.UI;

namespace Dylanng
{
    public abstract class UIPopup : UIBase
    {
        [Header("--- POPUP REFERENCES ---")] 
        [SerializeField] protected Button closeButton;

        protected override void OnInitialize()
        {
            if (closeButton) closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (closeButton) closeButton.onClick.RemoveAllListeners();
        }

        public virtual void OnCloseButtonClicked()
        {
            if (!IsOpen) return;

            ServiceLocator.Get<UIManager>()?.CloseCurrentPopup();
        }
    }

    public abstract class UIPopup<TData> : UIPopup, IUIData<TData>
    {
        public TData Data { get; private set; }

        public void Setup(TData data)
        {
            Data = data;
            OnSetup(data);
        }

        protected virtual void OnSetup(TData data) { }
    }
}
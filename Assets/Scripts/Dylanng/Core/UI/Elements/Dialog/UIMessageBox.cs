using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dylanng.Core.UI;

namespace Dylanng.Core.UI.Elements.Dialog
{
    public class UIMessageBox : UIPopup
    {
        [Header("Dialog Elements")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI confirmButtonText;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TextMeshProUGUI cancelButtonText;

        private Action _onConfirm;
        private Action _onCancel;

        public override void Initialize()
        {
            base.Initialize();
            
            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirmClicked);
            
            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancelClicked);
        }

        public override void Setup(object data = null)
        {
            base.Setup(data);
            
            if (data is DialogData dialogData)
            {
                if (titleText != null) titleText.text = string.IsNullOrEmpty(dialogData.Title) ? "Thông báo" : dialogData.Title;
                if (messageText != null) messageText.text = dialogData.Message;
                
                if (confirmButtonText != null) confirmButtonText.text = string.IsNullOrEmpty(dialogData.ConfirmText) ? "Đồng ý" : dialogData.ConfirmText;
                
                if (cancelButton != null)
                {
                    cancelButton.gameObject.SetActive(dialogData.ShowCancelButton);
                    if (cancelButtonText != null) cancelButtonText.text = string.IsNullOrEmpty(dialogData.CancelText) ? "Hủy" : dialogData.CancelText;
                }

                _onConfirm = dialogData.OnConfirm;
                _onCancel = dialogData.OnCancel;
            }
        }

        private void OnConfirmClicked()
        {
            _onConfirm?.Invoke();
            Close();
        }

        private void OnCancelClicked()
        {
            _onCancel?.Invoke();
            Close();
        }
    }
}

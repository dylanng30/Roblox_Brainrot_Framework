using System;

namespace Dylanng.Core.UI.Elements.Dialog
{
    public struct DialogData
    {
        public string Title;
        public string Message;
        public string ConfirmText;
        public string CancelText;
        public Action OnConfirm;
        public Action OnCancel;
        public bool ShowCancelButton;
    }
}

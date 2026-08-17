using System;

namespace Dylanng.Elements.Tab
{
    public class UITabPanel : UIBase
    {
        public event Action OnActive;
        public event Action OnDeactive;

        public void Activate()
        {
            Show();
            OnActive?.Invoke();
            OnTabActive();
        }

        public void Deactivate()
        {
            Hide();
            OnDeactive?.Invoke();
            OnTabDeactive();
        }

        protected virtual void OnTabActive()
        {
            // Hook for subclass implementation
        }

        protected virtual void OnTabDeactive()
        {
            // Hook for subclass implementation
        }
    }
}
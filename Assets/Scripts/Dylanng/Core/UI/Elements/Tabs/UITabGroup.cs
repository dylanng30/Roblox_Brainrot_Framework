using System.Collections.Generic;
using UnityEngine;

namespace Dylanng.Core.UI.Elements.Tabs
{
    public class UITabGroup : MonoBehaviour
    {
        [System.Serializable]
        public struct TabContentPair
        {
            public UITabButton button;
            public GameObject contentPanel;
        }

        [SerializeField] private List<TabContentPair> tabPairs = new List<TabContentPair>();
        [SerializeField] private int defaultTabIndex = 0;

        private UITabButton _selectedTab;
        private List<UITabButton> _subscribedButtons = new List<UITabButton>();

        public void Subscribe(UITabButton button)
        {
            if (!_subscribedButtons.Contains(button))
            {
                _subscribedButtons.Add(button);
            }
            
            // Auto select default tab when all initialized
            if (_subscribedButtons.Count == tabPairs.Count)
            {
                SelectTabByIndex(defaultTabIndex);
            }
        }

        public void OnTabSelected(UITabButton button)
        {
            if (_selectedTab == button) return;

            _selectedTab = button;
            
            foreach (var pair in tabPairs)
            {
                if (pair.button == null) continue;
                
                bool isSelected = pair.button == button;
                pair.button.SetSelected(isSelected);
                
                if (pair.contentPanel != null)
                {
                    pair.contentPanel.SetActive(isSelected);
                }
            }
        }

        public void SelectTabByIndex(int index)
        {
            if (index >= 0 && index < tabPairs.Count)
            {
                OnTabSelected(tabPairs[index].button);
            }
        }
    }
}

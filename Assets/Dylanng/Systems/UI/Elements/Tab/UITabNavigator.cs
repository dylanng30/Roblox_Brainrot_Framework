using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dylanng.Elements.Tab
{
    public class UITabNavigator : MonoBehaviourBase
    {
        [Serializable]
        public struct TabPair
        {
            public UITabButton tabButton;
            public UITabPanel tabPanel;
        }

        [Header("--- TAB CONFIGURATION ---")]
        [SerializeField] private List<TabPair> tabs = new List<TabPair>();
        [SerializeField] private int defaultTabIndex = 0;

        public event Action<int> OnTabChanged;

        public int CurrentTabIndex { get; private set; } = -1;

        protected override void Start()
        {
            base.Start();
            InitializeTabs();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CleanupTabs();
        }

        private void InitializeTabs()
        {
            if (tabs == null || tabs.Count == 0) return;

            for (int i = 0; i < tabs.Count; i++)
            {
                var pair = tabs[i];
                if (pair.tabButton != null)
                {
                    pair.tabButton.OnClicked += HandleTabClicked;
                }
            }
            
            if (defaultTabIndex >= 0 && defaultTabIndex < tabs.Count)
            {
                SelectTab(defaultTabIndex);
            }
        }

        private void CleanupTabs()
        {
            if (tabs == null) return;

            for (int i = 0; i < tabs.Count; i++)
            {
                var pair = tabs[i];
                if (pair.tabButton != null)
                {
                    pair.tabButton.OnClicked -= HandleTabClicked;
                }
            }
        }

        private void HandleTabClicked(UITabButton clickedButton)
        {
            SelectTab(clickedButton);
        }

        public void SelectTab(UITabButton button)
        {
            if (button == null) return;
            
            int index = tabs.FindIndex(pair => pair.tabButton == button);
            if (index != -1)
            {
                SelectTab(index);
            }
        }

        public void SelectTab(int index)
        {
            if (tabs == null || index < 0 || index >= tabs.Count) return;
            if (index == CurrentTabIndex) return;
            
            if (CurrentTabIndex >= 0 && CurrentTabIndex < tabs.Count)
            {
                var currentPair = tabs[CurrentTabIndex];
                if (currentPair.tabButton != null)
                    currentPair.tabButton.SetSelected(false);
                if (currentPair.tabPanel != null)
                    currentPair.tabPanel.Deactivate();
            }
            
            CurrentTabIndex = index;
            var newPair = tabs[index];
            
            if (newPair.tabButton != null)
                newPair.tabButton.SetSelected(true);
            if (newPair.tabPanel != null)
                newPair.tabPanel.Activate();

            OnTabChanged?.Invoke(index);
        }
    }
}
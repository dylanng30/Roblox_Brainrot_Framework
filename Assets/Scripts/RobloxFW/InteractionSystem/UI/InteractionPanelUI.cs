using System.Collections.Generic;
using Dylanng.Core;
using Dylanng.Core.UI;
using RobloxFW.InteractionSystem.Data;
using UnityEngine;

namespace RobloxFW.InteractionSystem.UI.Data
{
    public class InteractionPanelUI : UIBase
    {
        [SerializeField] private InteractionButton[] interactionButtonArray;
        private Dictionary<InteractionType, InteractionButton> _interactionButtonDic;

        public override void Initialize()
        {
            base.Initialize();

            _interactionButtonDic = new Dictionary<InteractionType, InteractionButton>();
            
            foreach (var interactionButton in interactionButtonArray)
            {
                if (!_interactionButtonDic.ContainsKey(interactionButton.InteractionType))
                {
                    _interactionButtonDic.Add(interactionButton.InteractionType, interactionButton);
                }
                else
                {
                    Debug.LogError($"Duplicate interaction type: {interactionButton.InteractionType}");
                }
            }
            
            EventBus.Subscribe<InteractionFocusChangedEvent>(OnFocusChanged);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<InteractionFocusChangedEvent>(OnFocusChanged);
        }

        private void OnFocusChanged(InteractionFocusChangedEvent evt)
        {
            if (evt.FocusedObject == null || 
                evt.AvailableInteractions == null || 
                evt.AvailableInteractions.Count == 0)
            {
                HideAllInteractions();
            }
            else
            {
                ShowInteractions(evt.AvailableInteractions);
            }
        }

        private void HideAllInteractions()
        {
            foreach (InteractionButton btn in _interactionButtonDic.Values)
            {
                btn.Hide();
            }
        }

        public void ShowInteractions(List<InteractionData> interactionDatas)
        {
            HideAllInteractions();
            
            if (interactionDatas == null) return;
            
            foreach (var data in interactionDatas)
            {
                if (_interactionButtonDic.TryGetValue(data.Type, out InteractionButton button))
                {
                    button.Show();
                    button.SetText(data.DisplayTexts);
                    button.SetupAction(data.OnInteractAction);
                }
            }
        }
    }
}
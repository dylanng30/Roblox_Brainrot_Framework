using System;
using System.Collections.Generic;
using Dylanng.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RobloxFW.InteractionSystem.UI
{
    public class InteractionButton : UIBase
    {
        [SerializeField] private InteractionType interactionType;
        [SerializeField] private Button interactionButton;
        [SerializeField] private List<TextMeshProUGUI> listInteractionButtonText;
            
        public InteractionType InteractionType => interactionType;

        private Action _callback;

        protected override void Awake()
        {
            base.Awake();
            
            if (interactionButton)
            {
                interactionButton.onClick.AddListener(OnInteractionButtonClicked);
            }
        }

        public void SetupAction(Action callback)
        {
            _callback = callback;
        }

        public void SetText(List<string> informations)
        {
            if(listInteractionButtonText == null ) return;
            if(listInteractionButtonText.Count == 0) return;
            
            for (int i = 0; i < listInteractionButtonText.Count; i++)
            {
                listInteractionButtonText[i].text = informations[i];
            }
        }

        private void OnInteractionButtonClicked()
        {
            _callback?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (interactionButton)
            {
                interactionButton.onClick.RemoveAllListeners();
            }
        }
    }
}
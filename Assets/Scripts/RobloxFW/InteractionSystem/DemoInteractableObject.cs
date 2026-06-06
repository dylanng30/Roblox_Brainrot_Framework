using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobloxFW.InteractionSystem
{
    public class DemoInteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private Outline _outline;
        public Transform Transform { get; set; }
        private List<InteractionData> _interactionDatas;
        private InteractionData _demoInteractionData;
        
        public List<InteractionData> GetAvailableInteractions(PlayerInteractor interactor)
        {
            _interactionDatas = _interactionDatas ?? new  List<InteractionData>();
            _interactionDatas.Clear();
            
            _demoInteractionData = _demoInteractionData ?? new InteractionData();
            _demoInteractionData.DisplayTexts.Clear();
            
            _demoInteractionData.Type = InteractionType.Demo;
            _demoInteractionData.DisplayTexts.Add("Demo Object");
            
            _interactionDatas.Add(_demoInteractionData);
            return _interactionDatas;
        }

        public bool CanInteract()
        {
            return true;
        }

        public void OnFocusGained()
        {
            // Enable Outline
        }

        public void OnFocusLost()
        {
            // Disable Outline
        }

        private void Awake()
        {
            Transform = transform;
        }
    }
}
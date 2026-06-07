using System.Collections.Generic;
using RobloxFW.InteractionSystem.Components;
using RobloxFW.InteractionSystem.Data;
using RobloxFW.InteractionSystem.Interfaces;
using UnityEngine;

namespace RobloxFW.InteractionSystem.Entities
{
    public class DemoInteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private Outline outline;
        public Transform Transform { get; set; }
        
        private List<InteractionData> _interactionDatas;
        private InteractionData _demoInteractionData;
        
        public List<InteractionData> GetAvailableInteractions(IEntityInteractor interactor)
        {
            _interactionDatas = _interactionDatas ?? new  List<InteractionData>();
            if(_interactionDatas.Count > 0) _interactionDatas.Clear();
            
            _demoInteractionData = _demoInteractionData ?? new InteractionData();
            if(_demoInteractionData.DisplayTexts.Count > 0) _demoInteractionData.DisplayTexts.Clear();

            if (interactor is PlayerInteractor playerInteractor)
            {
                _demoInteractionData.Type = InteractionType.Demo;
                _demoInteractionData.DisplayTexts.Add("Demo Object");
                _demoInteractionData.OnInteractAction = () =>
                {
                    playerInteractor.HandleDemo(this);
                };
                
                _interactionDatas.Add(_demoInteractionData);
            }
            
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
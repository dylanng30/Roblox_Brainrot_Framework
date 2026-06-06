using System.Collections.Generic;
using UnityEngine;

namespace RobloxFW.InteractionSystem
{
    public interface IInteractable
    {
        Transform Transform { get; set; }
        List<InteractionData> GetAvailableInteractions(PlayerInteractor interactor);
        bool CanInteract();
        void OnFocusGained();
        void OnFocusLost();
        
    }
}
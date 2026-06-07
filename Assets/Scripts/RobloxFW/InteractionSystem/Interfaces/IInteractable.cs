using System.Collections.Generic;
using RobloxFW.InteractionSystem.Components;
using RobloxFW.InteractionSystem.Data;
using UnityEngine;

namespace RobloxFW.InteractionSystem.Interfaces
{
    public interface IInteractable
    {
        Transform Transform { get; set; }
        List<InteractionData> GetAvailableInteractions(IEntityInteractor interactor);
        bool CanInteract();
        void OnFocusGained();
        void OnFocusLost();
        
    }
}
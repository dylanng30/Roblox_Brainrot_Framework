using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RobloxFW.InputSystem
{
    public class MobileActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action<bool> HoldAction;

        private bool isHolding;

        public void OnPointerDown(PointerEventData eventData)
        {
            HoldAction?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            HoldAction?.Invoke(false);
        }
    }
}

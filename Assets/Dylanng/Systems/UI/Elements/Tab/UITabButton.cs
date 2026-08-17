using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dylanng.Elements.Tab
{
    public class UITabButton : MonoBehaviourBase, IPointerClickHandler
    {
        [Header("--- VISUAL STATES ---")]
        [SerializeField] private GameObject selectedVisual;
        [SerializeField] private GameObject unselectedVisual;

        public event Action<UITabButton> OnClicked;
        
        public bool IsSelected { get; private set; }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            
            if (selectedVisual != null)
                selectedVisual.SetActive(isSelected);
                
            if (unselectedVisual != null)
                unselectedVisual.SetActive(!isSelected);
                
            OnSelectionChanged(isSelected);
        }

        protected virtual void OnSelectionChanged(bool isSelected)
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}
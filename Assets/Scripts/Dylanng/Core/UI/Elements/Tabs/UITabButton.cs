using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Dylanng.Core.UI.Elements.Tabs
{
    [RequireComponent(typeof(Image))]
    public class UITabButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UITabGroup tabGroup;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color activeColor = Color.gray;
        [SerializeField] private TextMeshProUGUI tabText;
        [SerializeField] private Color defaultTextColor = Color.black;
        [SerializeField] private Color activeTextColor = Color.white;

        private Image _background;

        private void Awake()
        {
            _background = GetComponent<Image>();
            if (tabGroup != null)
            {
                tabGroup.Subscribe(this);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (tabGroup != null)
            {
                tabGroup.OnTabSelected(this);
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (_background != null)
                _background.color = isSelected ? activeColor : defaultColor;
                
            if (tabText != null)
            {
                tabText.color = isSelected ? activeTextColor : defaultTextColor;
            }
        }
    }
}

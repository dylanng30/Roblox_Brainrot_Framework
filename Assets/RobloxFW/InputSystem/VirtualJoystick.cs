using UnityEngine;

namespace RobloxFW.InputSystem
{
    public class VirtualJoystick : MonoBehaviour
    {
        [Header("References")]
        public RectTransform background;
        public RectTransform handle;

        [Header("Settings")]
        [Range(0f, 2f)] public float handleLimit = 1f;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            if (background == null) background = GetComponent<RectTransform>();
            if (handle == null) handle = transform.GetChild(0).GetComponent<RectTransform>();

            _canvasGroup = background.GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = background.gameObject.AddComponent<CanvasGroup>();
            }

            Hide();
        }

        public void Show(Vector2 screenPosition)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            background.position = screenPosition;
            handle.anchoredPosition = Vector2.zero;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            handle.anchoredPosition = Vector2.zero;
        }

        public void UpdateHandle(Vector2 inputDirection)
        {
            if (background != null && handle != null)
            {
                handle.anchoredPosition = new Vector2(inputDirection.x * (background.sizeDelta.x / 2) * handleLimit,
                                                      inputDirection.y * (background.sizeDelta.y / 2) * handleLimit);
            }
        }

        public float GetRadius()
        {
            if (background != null) return background.sizeDelta.x / 2f;
            return Screen.width * 0.1f;
        }
    }
}

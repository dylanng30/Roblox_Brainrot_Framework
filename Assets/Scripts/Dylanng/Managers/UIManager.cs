using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Dylanng.Core.Base;
using Dylanng.Core.Data;
using Dylanng.Core.UI;

namespace Dylanng.Core.Managers
{
    public abstract class UIManager : ManagerBase
    {
        [Header("--- EDITOR SETTINGS ---")]
        [SerializeField] protected bool enableLog;
        
        [Space (10)]
        [Header("--- LAYER MANAGER ----")]
        [SerializeField] protected Transform topLayer;
        [SerializeField] protected Transform screenLayer;
        [SerializeField] protected Transform popupLayer;
        [SerializeField] protected Transform toastLayer;

        [Header("--- TOAST SETTINGS ---")]
        [SerializeField] private GameObject toastPrefab;

        [Header("--- UI DATABASE ---")]
        [SerializeField] private UiDatabaseSO uiDB;

        [Header("--- DELAY SETTINGS ---")]
        [SerializeField] private float openPopupDelay = 2f;
        
        protected Stack<UITop> _topStack;
        protected Stack<UITop> TopStack => _topStack ??= new Stack<UITop>();
        
        protected Stack<UIScreen> _screenStack;
        protected Stack<UIScreen> ScreenStack => _screenStack ??= new Stack<UIScreen>();

        protected Stack<UIPopup> _popupStack;
        protected Stack<UIPopup> PopupStack => _popupStack ??= new Stack<UIPopup>();

        protected Queue<Action> _popupQueue;
        protected Queue<Action> PopupQueue => _popupQueue ??= new Queue<Action>();

        protected Dictionary<Type, UIBase> _uiPrefabs;
        protected Dictionary<Type, UIBase> UIPrefabs => _uiPrefabs ??= BuildPrefabDictionary();

        protected Dictionary<Type, UIBase> _uiInstances;
        protected Dictionary<Type, UIBase> UIInstances => _uiInstances ??= new Dictionary<Type, UIBase>();

        protected WaitForSeconds _openPopupWait;
        protected Coroutine _openPopupCoroutine;

        public override void Initialize()
        {
            base.Initialize();

            _openPopupWait = new WaitForSeconds(openPopupDelay);
        }
        
        protected Dictionary<Type, UIBase> BuildPrefabDictionary()
        {
            var dict = new Dictionary<Type, UIBase>();
            if (uiDB != null && uiDB.UIs != null)
            {
                foreach (UIBase ui in uiDB.UIs)
                {
                    dict[ui.GetType()] = ui;
                }
            }
            return dict;
        }
        
        protected T GetOrInstantiateUI<T>(Transform parent) where T : UIBase
        {
            var type = typeof(T);
            if (UIInstances.TryGetValue(type, out var instance))
            {
                return instance as T;
            }

            if (UIPrefabs.TryGetValue(type, out var prefab))
            {
                try
                {
                    T newInstance = Instantiate(prefab as T, parent);
                    newInstance.Initialize();
                    UIInstances.Add(type, newInstance);
                    return newInstance;
                }
                catch (Exception ex)
                {
                    if (enableLog)
                    {
                        GameLogger.LogError($"[UIManager] Lỗi khi Instantiate {type.Name}: {ex.Message}");
                    }                    
                    return null;
                }
            }

            if (enableLog)
            {
                GameLogger.LogError($"[UIManager] Không tìm thấy Prefab của {type.Name} trong UiDatabaseSO!");
            }            
            return null;
        }

        #region --- TOP ---

        public T OpenTop<T>(object data = null, bool isImmediate = false) where T : UITop
        {
            T top = GetOrInstantiateUI<T>(topLayer);
            
            if (top != null)
            {
                if (TopStack.Count > 0)
                {
                    TopStack.Peek().Hide();
                }

                top.Setup(data);
                top.Show();
                TopStack.Push(top);
            }
            
            return top;
        }
        
        public void CloseCurrentTop()
        {
            if (_topStack == null || _topStack.Count == 0) return;

            var top = _topStack.Pop();
            top.Hide();
        }

        #endregion

        #region --- SCREEN MANAGEMENT ---
        
        public T OpenScreen<T>(object data = null) where T : UIScreen
        {
            T screen = GetOrInstantiateUI<T>(screenLayer);
            
            if (screen != null)
            {
                if (ScreenStack.Count > 0)
                {
                    ScreenStack.Peek().Hide();
                }

                screen.Setup(data);
                screen.Show();
                ScreenStack.Push(screen);
            }
            
            return screen;
        }

        public void CloseCurrentScreen()
        {
            if (_screenStack != null && _screenStack.Count > 1)
            {
                var screen = _screenStack.Pop();
                screen.Hide();

                _screenStack.Peek().Show();
            }
        }
        #endregion
        
        #region --- QUEUE POPUP ---
        
        public T OpenPopup<T>(object data = null, bool isImmediate = false) where T : UIPopup
        {
            bool isPopupActiveOrWaiting = (PopupStack.Count > 0) || (_openPopupCoroutine != null);

            if (!isImmediate && isPopupActiveOrWaiting)
            {
                PopupQueue.Enqueue(() => LoadAndShowPopup<T>(data));
                return null; 
            }
            
            return LoadAndShowPopup<T>(data);
        }

        protected T LoadAndShowPopup<T>(object data) where T : UIPopup
        {
            T popup = GetOrInstantiateUI<T>(popupLayer);
            
            if (popup != null)
            {
                popup.Setup(data);
                popup.Show();
                PopupStack.Push(popup);
            }

            return popup;
        }
        
        public void CloseCurrentPopup()
        {
            if (_popupStack == null || _popupStack.Count == 0) return;

            var popup = _popupStack.Pop();
            popup.Hide();
            
            if (_popupStack.Count == 0 && PopupQueue.Count > 0)
            {
                if (_openPopupCoroutine == null)
                {
                    _openPopupCoroutine = StartCoroutine(DelayOpenPopupCoroutine());
                }
            }
        }

        private IEnumerator DelayOpenPopupCoroutine()
        {
            yield return _openPopupWait;
            _openPopupCoroutine = null;
            if (PopupQueue.Count > 0)
            {
                var nextPopupAction = _popupQueue.Dequeue();
                nextPopupAction?.Invoke();
            }
        }

        public void CloseAllPopups()
        {
            if (_popupStack == null) return;
            
            while (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                popup.Hide();
            }
        }
        
        public void ClearPopupQueue()
        {
            _popupQueue?.Clear();
            
            if (_openPopupCoroutine != null)
            {
                StopCoroutine(_openPopupCoroutine);
                _openPopupCoroutine = null;
            }
        }

        #endregion
        
        #region --- TOAST MANAGEMENT ---

        private Queue<UI.Elements.Toast.UIToastItem> _toastPool;
        private Queue<UI.Elements.Toast.UIToastItem> ToastPool => _toastPool ??= new Queue<UI.Elements.Toast.UIToastItem>();

        public void ShowToast(string message, float duration = 2f)
        {
            if (toastPrefab == null || toastLayer == null)
            {
                GameLogger.LogError("[UIManager] Missing toastPrefab or toastLayer!");
                return;
            }

            UI.Elements.Toast.UIToastItem toast = null;
            if (ToastPool.Count > 0)
            {
                toast = ToastPool.Dequeue();
                toast.gameObject.SetActive(true);
            }
            else
            {
                var go = Instantiate(toastPrefab, toastLayer);
                toast = go.GetComponent<UI.Elements.Toast.UIToastItem>();
            }

            if (toast != null)
            {
                toast.transform.SetAsLastSibling();
                toast.ShowToast(message, duration, () => {
                    toast.gameObject.SetActive(false);
                    ToastPool.Enqueue(toast);
                });
            }
        }

        #endregion

        
    }
}
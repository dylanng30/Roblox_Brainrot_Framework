using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dylanng
{
    public class UIManager : ManagerBase
    {
        [Header("--- EDITOR SETTINGS ---")]
        [SerializeField] private bool enableLog;
        
        [Space (10)]
        [Header("--- LAYER MANAGER ----")]
        [SerializeField] private Transform topLayer;
        [SerializeField] private Transform screenLayer;
        [SerializeField] private Transform popupLayer;
        [SerializeField] private Transform toastLayer;

        [Header("--- TOAST SETTINGS ---")]
        [SerializeField] private GameObject toastPrefab;

        [Header("--- UI DATABASE ---")]
        [SerializeField] private UIDatabase uiDB;

        [Header("--- DELAY SETTINGS ---")]
        [SerializeField] private float openPopupDelay = 2f;
        
        private Stack<UITop> _topStack;
        private Stack<UITop> TopStack => _topStack ??= new Stack<UITop>();
        
        private Stack<UIScreen> _screenStack;
        private Stack<UIScreen> ScreenStack => _screenStack ??= new Stack<UIScreen>();

        private Stack<UIPopup> _popupStack;
        private Stack<UIPopup> PopupStack => _popupStack ??= new Stack<UIPopup>();

        private Queue<Action> _popupQueue;
        private Queue<Action> PopupQueue => _popupQueue ??= new Queue<Action>();

        private Dictionary<Type, UIBase> _uiPrefabs;
        private Dictionary<Type, UIBase> UIPrefabs => _uiPrefabs ??= BuildPrefabDictionary();

        private Dictionary<Type, UIBase> _uiInstances;
        private Dictionary<Type, UIBase> UIInstances => _uiInstances ??= new Dictionary<Type, UIBase>();

        private WaitForSeconds _openPopupWait;
        private Coroutine _openPopupCoroutine;

        public override void Initialize()
        {
            base.Initialize();

            _openPopupWait = new WaitForSeconds(openPopupDelay);
            
            ServiceLocator.Register(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Unregister<UIManager>();
        }

         Dictionary<Type, UIBase> BuildPrefabDictionary()
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
        
        private T GetOrInstantiateUI<T>(Transform parent) where T : UIBase
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

        public T OpenTop<T>(bool isImmediate = false) where T : UITop
        {
            if (typeof(IUIData).IsAssignableFrom(typeof(T)))
            {
                GameLogger.LogError($"[UIManager] LỖI: {typeof(T).Name} yêu cầu Data! Hãy dùng OpenTop<{typeof(T).Name}, TData>(data).");
                return null;
            }

            T top = GetOrInstantiateUI<T>(topLayer);
            if (top != null)
            {
                if (top.IsOpened || top.IsOpening) return top;

                if (TopStack.Count > 0) TopStack.Peek().Hide();
                top.Show();
                TopStack.Push(top);
            }
            return top;
        }

        public T OpenTop<T, TData>(TData data, bool isImmediate = false) where T : UITop, IUIData<TData>
        {
            T top = GetOrInstantiateUI<T>(topLayer);
            if (top != null)
            {
                if (top.IsOpened || top.IsOpening)
                {
                    top.Setup(data);
                    return top;
                }

                if (TopStack.Count > 0) TopStack.Peek().Hide();
                top.Setup(data);
                top.Show();
                TopStack.Push(top);
            }
            return top;
        }
        
        public void CloseCurrentTop()
        {
            if (_topStack == null || _topStack.Count == 0) return;

            var top = _topStack.Peek();
            if (top.IsClosed || top.IsClosing) return;

            top = _topStack.Pop();
            top.Hide();
        }

        #endregion

        #region --- SCREEN MANAGEMENT ---
        
        public T OpenScreen<T>() where T : UIScreen
        {
            if (typeof(IUIData).IsAssignableFrom(typeof(T)))
            {
                GameLogger.LogError($"[UIManager] LỖI: {typeof(T).Name} yêu cầu Data! Hãy dùng OpenScreen<{typeof(T).Name}, TData>(data).");
                return null;
            }

            T screen = GetOrInstantiateUI<T>(screenLayer);
            if (screen != null)
            {
                if (screen.IsOpened || screen.IsOpening) return screen;

                if (ScreenStack.Count > 0) ScreenStack.Peek().Hide();
                screen.Show();
                ScreenStack.Push(screen);
            }
            return screen;
        }

        public T OpenScreen<T, TData>(TData data) where T : UIScreen, IUIData<TData>
        {
            T screen = GetOrInstantiateUI<T>(screenLayer);
            if (screen != null)
            {
                if (screen.IsOpened || screen.IsOpening)
                {
                    screen.Setup(data);
                    return screen;
                }

                if (ScreenStack.Count > 0) ScreenStack.Peek().Hide();
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
                var screen = _screenStack.Peek();
                if (screen.IsClosed || screen.IsClosing) return;

                screen = _screenStack.Pop();
                screen.Hide();

                _screenStack.Peek().Show();
            }
        }
        #endregion
        
        #region --- QUEUE POPUP ---
        
        public T OpenPopup<T>(bool isImmediate = false) where T : UIPopup
        {
            if (typeof(IUIData).IsAssignableFrom(typeof(T)))
            {
                GameLogger.LogError($"[UIManager] LỖI: {typeof(T).Name} yêu cầu Data! Hãy dùng OpenPopup<{typeof(T).Name}, TData>(data).");
                return null;
            }

            bool isPopupActiveOrWaiting = (PopupStack.Count > 0) || (_openPopupCoroutine != null);

            if (!isImmediate && isPopupActiveOrWaiting)
            {
                PopupQueue.Enqueue(() => LoadAndShowPopup<T>());
                return null; 
            }
            
            return LoadAndShowPopup<T>();
        }

        public T OpenPopup<T, TData>(TData data, bool isImmediate = false) where T : UIPopup, IUIData<TData>
        {
            bool isPopupActiveOrWaiting = (PopupStack.Count > 0) || (_openPopupCoroutine != null);

            if (!isImmediate && isPopupActiveOrWaiting)
            {
                PopupQueue.Enqueue(() => LoadAndShowPopup<T, TData>(data));
                return null; 
            }
            
            return LoadAndShowPopup<T, TData>(data);
        }

        private T LoadAndShowPopup<T>() where T : UIPopup
        {
            T popup = GetOrInstantiateUI<T>(popupLayer);
            if (popup != null)
            {
                if (popup.IsOpened || popup.IsOpening) return popup;
                popup.Show();
                PopupStack.Push(popup);
            }
            return popup;
        }

        private T LoadAndShowPopup<T, TData>(TData data) where T : UIPopup, IUIData<TData>
        {
            T popup = GetOrInstantiateUI<T>(popupLayer);
            if (popup != null)
            {
                if (popup.IsOpened || popup.IsOpening)
                {
                    popup.Setup(data);
                    return popup;
                }
                popup.Setup(data);
                popup.Show();
                PopupStack.Push(popup);
            }
            return popup;
        }
        
        public void CloseCurrentPopup()
        {
            if (_popupStack == null || _popupStack.Count == 0) return;

            var popup = _popupStack.Peek();
            if (popup.IsClosed || popup.IsClosing) return;

            popup = _popupStack.Pop();
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
    }
}
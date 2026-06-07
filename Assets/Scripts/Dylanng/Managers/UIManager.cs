using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Dylanng.Core.Base;
using Dylanng.Core.UI;

namespace Dylanng.Core.Managers
{
    public class UIManager : ManagerBase
    {
        [Header("Layer Management")]
        [SerializeField] private Transform _screenLayer;
        [SerializeField] private Transform _popupLayer;
        [SerializeField] private Transform _loadingLayer;
        
        [Header("Loading Indicator")]
        [SerializeField] private GameObject _loadingIndicatorPrefab;
        
        private readonly Stack<UIScreen> _screenStack = new Stack<UIScreen>();
        private readonly Stack<UIPopup> _popupStack = new Stack<UIPopup>();
        
        private readonly Queue<Func<Task>> _popupQueue = new Queue<Func<Task>>();
        private readonly Dictionary<Type, UIBase> _registeredUIs = new Dictionary<Type, UIBase>();

        public const string ScreensPath = "UI/Screens/";
        public const string PopupsPath = "UI/Popups/";

        private bool _isTransitioning;
        private GameObject _activeLoadingIndicator;

        public override void Initialize()
        {
            ServiceLocator.Register<UIManager>(this);
        }

        #region --- ASYNC LOADINNG ---
        private async Task<T> LoadUIAsync<T>(string path, Transform parent) where T : UIBase
        {
            var type = typeof(T);
            
            if (_registeredUIs.TryGetValue(type, out var view))
            {
                return view as T;
            }

            try
            {
                ShowLoadingIndicator();
                
                ResourceRequest request = Resources.LoadAsync<T>(path);
                
                while (!request.isDone)
                {
                    await Task.Yield();
                }

                T uiPrefab = request.asset as T;
                if (uiPrefab == null)
                {
                    GameLogger.LogError($"[UIManager] Không tìm thấy Prefab tại đường dẫn: {path}");
                    return null;
                }
                
                T instance = Instantiate(uiPrefab, parent);
                instance.Initialize();
                _registeredUIs.Add(type, instance);
                
                return instance;
            }
            catch (Exception ex)
            {
                GameLogger.LogError($"[UIManager] Lỗi khi load Async {type.Name}: {ex.Message}");
                return null;
            }
            finally
            {
                HideLoadingIndicator();
            }
        }

        private void ShowLoadingIndicator()
        {
            if (_loadingIndicatorPrefab != null && _activeLoadingIndicator == null)
            {
                _activeLoadingIndicator = Instantiate(_loadingIndicatorPrefab, _loadingLayer);
            }
            else if (_activeLoadingIndicator != null)
            {
                _activeLoadingIndicator.SetActive(true);
            }
        }

        private void HideLoadingIndicator()
        {
            if (_activeLoadingIndicator != null)
            {
                _activeLoadingIndicator.SetActive(false);
            }
        }

        #endregion
        
        #region --- Screen Management ---
        public async Task<T> OpenScreenAsync<T>(object data = null) where T : UIScreen
        {
            if (_isTransitioning) return null;
            _isTransitioning = true;

            T screen = await LoadUIAsync<T>($"{ScreensPath}{typeof(T).Name}", _screenLayer);
            
            if (screen != null)
            {
                if (_screenStack.Count > 0)
                {
                    _screenStack.Peek().Hide();
                }

                screen.Setup(data);
                screen.Show();
                _screenStack.Push(screen);
            }

            _isTransitioning = false;
            return screen;
        }

        public void CloseCurrentScreen()
        {
            if (_isTransitioning) return;

            if (_screenStack.Count > 1)
            {
                var screen = _screenStack.Pop();
                screen.Hide();

                _screenStack.Peek().Show();
            }
        }

        #endregion

        #region --- QUEUE POPUP ---
        public async Task<T> OpenPopupAsync<T>(object data = null, bool isImmediate = false) where T : UIPopup
        {
            if (!isImmediate && _popupStack.Count > 0)
            {
                Task EnqueuePopupAction() => LoadAndShowPopupAsync<T>(data);
                
                _popupQueue.Enqueue(EnqueuePopupAction);
                return null; 
            }
            
            return await LoadAndShowPopupAsync<T>(data);
        }

        private async Task<T> LoadAndShowPopupAsync<T>(object data) where T : UIPopup
        {
            while (_isTransitioning) await Task.Yield();
            _isTransitioning = true;

            T popup = await LoadUIAsync<T>($"{PopupsPath}{typeof(T).Name}", _popupLayer);
            
            if (popup != null)
            {
                popup.Setup(data);
                popup.Show();
                _popupStack.Push(popup);
            }

            _isTransitioning = false;
            return popup;
        }
        
        public async void CloseCurrentPopup()
        {
            if (_isTransitioning || _popupStack.Count == 0) return;

            var popup = _popupStack.Pop();
            popup.Hide();
            
            if (_popupStack.Count == 0 && _popupQueue.Count > 0)
            {
                var nextPopupTask = _popupQueue.Dequeue();
                if (nextPopupTask != null)
                {
                    await nextPopupTask.Invoke();
                }
            }
        }

        public void CloseAllPopups()
        {
            while (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                popup.Hide();
            }
        }
        
        public void ClearPopupQueue()
        {
            _popupQueue.Clear();
        }

        #endregion
        
    }
}
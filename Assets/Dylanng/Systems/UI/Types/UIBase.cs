using System;
using UnityEngine;

namespace Dylanng
{
    public enum UIState
    {
        Closed,
        Opening,
        Opened,
        Closing
    }

    public interface IUIData { }

    public interface IUIData<TData> : IUIData
    {
        void Setup(TData data);
    }

    public abstract class UIBase : MonoBehaviourBase
    {
        [Header("--- TRANSITIONS ---")]
        [SerializeField] private UITransitionComponent[] transitions;

        public UIState CurrentState { get; private set; } = UIState.Closed;

        public bool IsClosed => CurrentState == UIState.Closed;
        public bool IsOpening => CurrentState == UIState.Opening;
        public bool IsOpened => CurrentState == UIState.Opened;
        public bool IsClosing => CurrentState == UIState.Closing;

        public bool IsOpen => IsOpened || IsOpening;
        
        private bool _isInitialized = false;
        private bool _isHideQueued = false;
        private bool _isShowQueued = false;

        private Action _onOpenTransitionComplete;
        private Action _onCloseTransitionComplete;
        private int _completedTransitions;

        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            _onOpenTransitionComplete = OnOpenTransitionComplete;
            _onCloseTransitionComplete = OnCloseTransitionComplete;

            OnInitialize();
        }

        public void Show()
        {
            if (IsOpened || IsOpening) return;

            if (IsClosing)
            {
                _isShowQueued = true;
                return;
            }
            
            CurrentState = UIState.Opening;
            gameObject.SetActive(true);
            BeforeShow();
            PlayShowAnimation();
        }

        public void Hide()
        {
            if (IsClosed || IsClosing) return;

            if (IsOpening)
            {
                _isHideQueued = true;
                return;
            }
            
            CurrentState = UIState.Closing;
            BeforeHide();
            PlayHideAnimation();
        }

        protected virtual void OnInitialize() { }
        
        protected virtual void BeforeShow() 
        {
            if (transitions != null)
            {
                for (int i = 0; i < transitions.Length; i++)
                    transitions[i].SetBeforeOpenState();
            }
        }
        
        protected virtual void PlayShowAnimation() 
        {
            if (transitions == null || transitions.Length == 0)
            {
                AfterShow();
                return;
            }

            _completedTransitions = 0;
            for (int i = 0; i < transitions.Length; i++)
            {
                transitions[i].PlayOpen(_onOpenTransitionComplete);
            }
        }

        private void OnOpenTransitionComplete()
        {
            _completedTransitions++;
            if (_completedTransitions >= transitions.Length)
            {
                AfterShow();
            }
        }
        
        protected virtual void AfterShow() 
        { 
            CurrentState = UIState.Opened;
            if (_isHideQueued)
            {
                _isHideQueued = false;
                Hide();
            }
        }
        
        protected virtual void BeforeHide() 
        { 
            if (transitions != null)
            {
                for (int i = 0; i < transitions.Length; i++)
                    transitions[i].SetBeforeCloseState();
            }
        }
        
        protected virtual void PlayHideAnimation()
        {
            if (transitions == null || transitions.Length == 0)
            {
                AfterHide();
                return;
            }

            _completedTransitions = 0;
            for (int i = 0; i < transitions.Length; i++)
            {
                transitions[i].PlayClose(_onCloseTransitionComplete);
            }
        }

        private void OnCloseTransitionComplete()
        {
            _completedTransitions++;
            if (_completedTransitions >= transitions.Length)
            {
                AfterHide();
            }
        }
        
        protected virtual void AfterHide() 
        {
            CurrentState = UIState.Closed;
            gameObject.SetActive(false);
            
            if (_isShowQueued)
            {
                _isShowQueued = false;
                Show();
            }
        }
    }

    public abstract class UIBase<TData> : UIBase, IUIData<TData>
    {
        public TData Data { get; private set; }

        public void Setup(TData data)
        {
            Data = data;
            OnSetup(data);
        }

        protected virtual void OnSetup(TData data) { }
    }
}
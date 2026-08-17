using System;
using System.Collections.Generic;

namespace Dylanng
{
    public interface IEvent {}
    
    public static class EventBus
    {
        private static readonly List<Action> _clearActions = new();

        internal static void RegisterClearAction(Action clearAction)
        {
            _clearActions.Add(clearAction);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < _clearActions.Count; i++)
            {
                _clearActions[i]?.Invoke();
            }
        }

        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            EventBusInternal<T>.Subscribe(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            EventBusInternal<T>.Unsubscribe(handler);
        }

        public static void Publish<T>(T eventData) where T : IEvent
        {
            EventBusInternal<T>.Publish(eventData);
        }
    }

    internal static class EventBusInternal<T> where T : IEvent
    {
        private static readonly List<Action<T>> _handlers = new List<Action<T>>();

        static EventBusInternal()
        {
            EventBus.RegisterClearAction(Clear);
        }

        public static void Subscribe(Action<T> handler)
        {
            if (!_handlers.Contains(handler))
            {
                _handlers.Add(handler);
            }
        }

        public static void Unsubscribe(Action<T> handler)
        {
            _handlers.Remove(handler);
        }

        public static void Publish(T eventData)
        {
            for (int i = _handlers.Count - 1; i >= 0; i--)
            {
                try
                {
                    _handlers[i].Invoke(eventData);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"[EventBus] Error in handler for event {typeof(T).Name}: {e}");
                }
            }
        }

        public static void Clear()
        {
            _handlers.Clear();
        }
    }
}

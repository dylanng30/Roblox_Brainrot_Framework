using System;
using System.Collections.Generic;

namespace Dylanng.Core
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> subs = new ();

        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            Type t = typeof(T);
            if (!subs.TryGetValue(t, out List<Delegate> list))
            {
                list = new List<Delegate>();
                subs[t] = list;
            }
            
            if (!list.Contains(handler))
            {
                list.Add(handler);
            }
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            Type t = typeof(T);
            if (subs.TryGetValue(t, out List<Delegate> list))
            {
                list.Remove(handler);
            }
        }

        public static void Publish<T>(T eventData) where T : IEvent
        {
            Type t = typeof(T);
            if (subs.TryGetValue(t, out List<Delegate> list))
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    ((Action<T>)list[i]).Invoke(eventData);
                }
            }
        }
        
    }
}
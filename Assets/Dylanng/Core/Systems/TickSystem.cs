using System;
using System.Collections.Generic;

namespace Dylanng
{
    public class TickSystem : SystemBase, ITickSystem
    {
        private readonly List<IUpdatable> updatables = new List<IUpdatable>();
        private readonly List<IFixedUpdatable> fixedUpdatables = new List<IFixedUpdatable>();
        private readonly List<ILateUpdatable> lateUpdatables = new List<ILateUpdatable>();
        
        private readonly HashSet<object> _toAdd = new HashSet<object>();
        private readonly HashSet<object> _toRemove = new HashSet<object>();

        public override void Initialize()
        {
            ServiceLocator.Register<ITickSystem>(this);
        }

        public override void Cleanup()
        {
            ServiceLocator.Unregister<ITickSystem>();
        }

        public void ClearAllTickables()
        {
            updatables.Clear();
            fixedUpdatables.Clear();
            lateUpdatables.Clear();
            
            _toAdd.Clear();
            _toRemove.Clear();
        }

        public void Register(object tickable)
        {
            if (tickable == null) return;
            _toAdd.Add(tickable);
            _toRemove.Remove(tickable);
        }

        public void Unregister(object tickable)
        {
            if (tickable == null) return;
            _toRemove.Add(tickable);
            _toAdd.Remove(tickable);
        }
        
        public void UpdateTicks(float deltaTime)
        {
            ProcessPendingChanges();
            
            for (int i = updatables.Count - 1; i >= 0; i--)
            {
                try
                {
                    updatables[i].OnUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    GameLogger.LogError($"Error in Update of {updatables[i].GetType().Name}: {e.Message}");
                }
            }
        }

        public void FixedUpdateTicks(float fixedDeltaTime)
        {
            for (int i = fixedUpdatables.Count - 1; i >= 0; i--)
            {
                try
                {
                    fixedUpdatables[i].OnFixedUpdate(fixedDeltaTime);
                }
                catch (Exception e)
                {
                    GameLogger.LogError($"Error in FixedUpdate: {e.ToString()}");
                }
            }
        }

        public void LateUpdateTicks(float deltaTime)
        {
            for (int i = lateUpdatables.Count - 1; i >= 0; i--)
            {
                try
                {
                    lateUpdatables[i].OnLateUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    GameLogger.LogError($"Error in LateUpdate: {e.Message}");
                }
            }
        }
        
        private void ProcessPendingChanges()
        {
            if (_toRemove.Count > 0)
            {
                foreach (var obj in _toRemove)
                {
                    if (obj is IUpdatable u) updatables.Remove(u);
                    if (obj is IFixedUpdatable fu) fixedUpdatables.Remove(fu);
                    if (obj is ILateUpdatable lu) lateUpdatables.Remove(lu);
                }

                _toRemove.Clear();
            }

            if (_toAdd.Count > 0)
            {
                foreach (var obj in _toAdd)
                {
                    if (obj is IUpdatable u && !updatables.Contains(u)) updatables.Add(u);
                    if (obj is IFixedUpdatable fu && !fixedUpdatables.Contains(fu)) fixedUpdatables.Add(fu);
                    if (obj is ILateUpdatable lu && !lateUpdatables.Contains(lu)) lateUpdatables.Add(lu);
                }

                _toAdd.Clear();
            }
        }
    }
}
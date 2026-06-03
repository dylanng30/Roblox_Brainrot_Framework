using System;
using System.Collections.Generic;
using Dylanng.Core.Base;

namespace Dylanng.Core.Systems.TickSystem
{
    public class TickSystem : SystemBase, ITickSystem
    {
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        private readonly List<IFixedUpdatable> _fixedUpdatables = new List<IFixedUpdatable>();
        private readonly List<ILateUpdatable> _lateUpdatables = new List<ILateUpdatable>();
        
        private readonly HashSet<object> _toAdd = new HashSet<object>();
        private readonly HashSet<object> _toRemove = new HashSet<object>();

        public override void Initialize()
        {
            ServiceLocator.Register<ITickSystem>(this);
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
            
            for (int i = _updatables.Count - 1; i >= 0; i--)
            {
                try
                {
                    _updatables[i].OnUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    GameLogger.LogError($"Error in Update of {_updatables[i].GetType().Name}: {e.Message}");
                }
            }
        }

        public void FixedUpdateTicks(float fixedDeltaTime)
        {
            for (int i = _fixedUpdatables.Count - 1; i >= 0; i--)
            {
                try
                {
                    _fixedUpdatables[i].OnFixedUpdate(fixedDeltaTime);
                }
                catch (Exception e)
                {
                    GameLogger.LogError($"Error in FixedUpdate: {e.Message}");
                }
            }
        }

        public void LateUpdateTicks(float deltaTime)
        {
            for (int i = _lateUpdatables.Count - 1; i >= 0; i--)
            {
                try
                {
                    _lateUpdatables[i].OnLateUpdate(deltaTime);
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
                    if (obj is IUpdatable u) _updatables.Remove(u);
                    if (obj is IFixedUpdatable fu) _fixedUpdatables.Remove(fu);
                    if (obj is ILateUpdatable lu) _lateUpdatables.Remove(lu);
                }

                _toRemove.Clear();
            }

            if (_toAdd.Count > 0)
            {
                foreach (var obj in _toAdd)
                {
                    if (obj is IUpdatable u && !_updatables.Contains(u)) _updatables.Add(u);
                    if (obj is IFixedUpdatable fu && !_fixedUpdatables.Contains(fu)) _fixedUpdatables.Add(fu);
                    if (obj is ILateUpdatable lu && !_lateUpdatables.Contains(lu)) _lateUpdatables.Add(lu);
                }

                _toAdd.Clear();
            }
        }
    }
}
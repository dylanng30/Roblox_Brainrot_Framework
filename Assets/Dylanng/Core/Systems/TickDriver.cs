using UnityEngine;

namespace Dylanng
{
    public class TickDriver : MonoBehaviourBase
    {
        private TickSystem _tickSystem;
        private PauseSystem _pauseSystem;

        public void Initialize(TickSystem tickSystem)
        {
            _tickSystem = tickSystem;
            _pauseSystem = ServiceLocator.Get<PauseSystem>();
        }

        private void Update()
        {
            if (_tickSystem == null || (_pauseSystem != null && _pauseSystem.IsPaused)) return;
            _tickSystem.UpdateTicks(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_tickSystem == null || (_pauseSystem != null && _pauseSystem.IsPaused)) return;
            _tickSystem.FixedUpdateTicks(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (_tickSystem == null || (_pauseSystem != null && _pauseSystem.IsPaused)) return;
            _tickSystem.LateUpdateTicks(Time.deltaTime);
        }
    
    
    }
}
using UnityEngine;

namespace Dylanng
{
    public class TickManager : ManagerBase
    {
        // REFERENCES
        private TickSystem _tickSystem;
        private PauseSystem _pauseSystem;

        private bool _canTick => _tickSystem != null && 
                                 _pauseSystem != null && 
                                 !_pauseSystem.IsPaused;

        protected override void OnGameBooted(IGameBootedEvent evt)
        {
            base.OnGameBooted(evt);
            
            _pauseSystem = ServiceLocator.Get<PauseSystem>();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _tickSystem = new TickSystem();
            _tickSystem.Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _tickSystem.Cleanup();
        }

        private void Update()
        {
            if (_canTick) _tickSystem.UpdateTicks(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_canTick) _tickSystem.FixedUpdateTicks(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (_canTick) _tickSystem.LateUpdateTicks(Time.deltaTime);
        }
    }
}
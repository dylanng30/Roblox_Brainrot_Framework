using UnityEngine;

namespace Dylanng
{
    public class PauseSystem : SystemBase
    {
        public bool IsPaused { get; private set; }

        public override void Initialize()
        {
            ServiceLocator.Register(this);
            IsPaused = false;
        }

        public override void Cleanup()
        {
            ServiceLocator.Unregister<PauseSystem>();
        }

        public void TogglePause()
        {
            SetPause(!IsPaused);
        }

        public void SetPause(bool isPaused)
        {
            if (IsPaused == isPaused) return;

            IsPaused = isPaused;
            Time.timeScale = IsPaused ? 0f : 1f;
            
            EventBus.Publish(new GamePausedEvent(IsPaused));
        }
        
    }
}
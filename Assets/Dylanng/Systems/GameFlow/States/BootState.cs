using UnityEngine;

namespace Dylanng
{
    public class BootState : IGameState
    {
        public void Enter()
        {
            GameLogger.Log("Entered Boot State");
            // Do bootstrapping, loading initial assets, etc.
        }

        public void Update()
        {
        }

        public void Exit()
        {
            GameLogger.Log("Exiting Boot State");
        }
    }
}

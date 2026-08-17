using UnityEngine;

namespace Dylanng
{
    public class PlayingState : IGameState
    {
        public void Enter()
        {
            GameLogger.Log("Entered Playing State");
            // Initialize level, player, etc.
        }

        public void Update()
        {
        }

        public void Exit()
        {
            GameLogger.Log("Exiting Playing State");
        }
    }
}

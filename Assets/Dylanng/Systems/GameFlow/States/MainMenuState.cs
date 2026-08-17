using UnityEngine;

namespace Dylanng
{
    public class MainMenuState : IGameState
    {
        public void Enter()
        {
            GameLogger.Log("Entered Main Menu State");
            // Load main menu UI
        }

        public void Update()
        {
        }

        public void Exit()
        {
            GameLogger.Log("Exiting Main Menu State");
        }
    }
}

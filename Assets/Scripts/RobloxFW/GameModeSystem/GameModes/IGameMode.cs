namespace RobloxFW.GameModeSystem.GameModes
{
    public interface IGameMode
    {
        void Initialize();
        void StartGame();
        void Cleanup();
    }
}
using RobloxFW.GameModeSystem.GameModes;
using UnityEngine;

namespace RobloxFW.GameModeSystem.SceneBootstrappers
{
    [DefaultExecutionOrder(-100)]
    public abstract class SceneBootstrapper : MonoBehaviour
    {
        protected IGameMode CurrentGameMode { get; private set; }
        private void Awake()
        {
            RegisterModeServices();

            InitializeEnvironment();

            CurrentGameMode = CreateGameMode();

            CurrentGameMode.Initialize();
        }
        private void Start()
        {
            CurrentGameMode.StartGame();
        }
        private void OnDestroy()
        {
            CurrentGameMode?.Cleanup();
            UnregisterModeServices();
        }

        protected abstract void RegisterModeServices();
        protected abstract void UnregisterModeServices();
        protected abstract IGameMode CreateGameMode();
        
        /// <summary>
        /// (Optional) Khởi tạo Pools hoặc Map.
        /// </summary>
        protected virtual void InitializeEnvironment() { }
    }
}
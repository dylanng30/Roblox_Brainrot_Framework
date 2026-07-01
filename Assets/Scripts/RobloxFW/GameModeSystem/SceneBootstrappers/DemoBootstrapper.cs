using RobloxFW.GameModeSystem.GameModes;
using RobloxFW.GameModeSystem.Demo;
using UnityEngine;

namespace RobloxFW.GameModeSystem.SceneBootstrappers
{
    public class DemoBootstrapper: SceneBootstrapper
    {
        [Header("Mode Configuration")]
        [SerializeField] private GameObject _demoPlayerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        
        private IDemoScoreService _scoreService;

        protected override void RegisterModeServices()
        {
            
            _scoreService = new DemoScoreService();
            // ServiceLocator.Register<IDemoScoreService>(_scoreService);
        }

        protected override void UnregisterModeServices()
        {
            // ServiceLocator.Unregister<IDemoScoreService>();
            _scoreService = null;
        }

        protected override void InitializeEnvironment()
        {
            // PoolManager.CreatePool("Enemy", _enemyPrefab, 10);
        }

        protected override IGameMode CreateGameMode()
        {
            return new DemoGameMode(_demoPlayerPrefab, _playerSpawnPoint);
        }
    }
}
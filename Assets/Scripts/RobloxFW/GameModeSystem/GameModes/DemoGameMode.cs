using UnityEngine;
using RobloxFW.GameModeSystem.Demo;

namespace RobloxFW.GameModeSystem.GameModes
{
    public class DemoGameMode : IGameMode
    {
        private readonly GameObject _playerPrefab;
        private readonly Transform _spawnPoint;
        private DemoPlayerController _currentPlayer;

        public DemoGameMode(GameObject playerPrefab, Transform spawnPoint)
        {
            _playerPrefab = playerPrefab;
            _spawnPoint = spawnPoint;
        }

        public void Initialize()
        {
            
            if (_playerPrefab != null)
            {
                var position = _spawnPoint != null ? _spawnPoint.position : Vector3.zero;
                var rotation = _spawnPoint != null ? _spawnPoint.rotation : Quaternion.identity;
                
                var playerObj = Object.Instantiate(_playerPrefab, position, rotation);
                _currentPlayer = playerObj.GetComponent<DemoPlayerController>();
                _currentPlayer?.Setup();
            }
            else
            {
                Debug.LogWarning("[DemoGameMode] Player Prefab is null");
            }

            // EventBus.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
        }

        public void StartGame()
        {
            _currentPlayer?.EnableInput(true);

            // var scoreService = ServiceLocator.Resolve<IDemoScoreService>();
            // scoreService.AddScore(100);
            
            Debug.Log("<color=green>[DemoGameMode]</color> Mode started");
        }

        public void Cleanup()
        {            
            _currentPlayer?.EnableInput(false);

            if (_currentPlayer != null)
            {
                Object.Destroy(_currentPlayer.gameObject);
            }

            // EventBus.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
            
            Debug.Log("<color=red>[DemoGameMode]</color> Clean up");
        }
    }
}
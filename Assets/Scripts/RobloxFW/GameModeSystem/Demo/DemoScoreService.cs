using UnityEngine;

namespace RobloxFW.GameModeSystem.Demo
{
    public interface IDemoScoreService
    {
        void AddScore(int amount);
        int GetScore();
    }

    public class DemoScoreService : IDemoScoreService
    {
        private int _score;
        public void AddScore(int amount)
        {
            _score += amount;
            Debug.Log($"[DemoScoreService] Đã cộng {amount} điểm. Tổng điểm hiện tại: {_score}");
        }

        public int GetScore() => _score;
    }
}

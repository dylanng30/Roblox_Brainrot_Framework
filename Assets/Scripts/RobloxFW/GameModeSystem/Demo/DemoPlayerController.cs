using UnityEngine;

namespace RobloxFW.GameModeSystem.Demo
{
    public class DemoPlayerController : MonoBehaviour
    {
        public void Setup()
        {
            Debug.Log("[DemoPlayerController] Player đã được khởi tạo và nhận references (Vũ khí, Camera...).");
        }

        public void EnableInput(bool enable)
        {
            Debug.Log($"[DemoPlayerController] Trạng thái Input hệ thống: {(enable ? "BẬT" : "TẮT")}.");
        }
    }
}

using RobloxFW.MovementSystem.Utilities;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.Components
{
    public class PlayerColliderDetectors : MonoBehaviour
    {
        //Hands
        [SerializeField] private LayerMask _groundLayer;
        
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _rightHand;
        //Foots
        [SerializeField] private Transform _leftFoot;
        [SerializeField] private Transform _rightFoot;
        
        public WallDetector WallDetector { get; private set; }
        public GroundDetector GroundDetector { get; private set; }

        public void Initialize()
        {
            InitizalizeWallDetector();
            InitizalizeGroundDetector();
        }

        private void InitizalizeGroundDetector()
        {
            GroundDetector = new GroundDetector();
            
            if (_leftFoot == null || _rightFoot == null)
            {
                Debug.LogError($"Left foot: {_leftFoot} / Right foot: {_rightFoot}");
                return;
            }
            
            GroundDetector.RegisterCheckPoint(_leftFoot, _rightFoot, _groundLayer);
        }
        private void InitizalizeWallDetector()
        {
            WallDetector = new WallDetector();
            
            if (_leftHand == null || _rightHand == null)
            {
                Debug.LogError($"Left foot: {_leftFoot} / Right foot: {_rightHand}");
                return;
            }
            
            WallDetector.RegisterCheckPoint(_leftHand, _rightHand, _groundLayer);
        }
    }
}
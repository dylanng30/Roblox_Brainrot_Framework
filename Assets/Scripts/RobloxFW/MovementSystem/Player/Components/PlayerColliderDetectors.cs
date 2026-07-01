using RobloxFW.MovementSystem.Utilities;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.Components
{
    public class PlayerColliderDetectors : MonoBehaviour
    {
        //Foots
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;
        
        //Hands
        /*[SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _rightHand;*/
        
        
        //public WallDetector WallDetector { get; private set; }
        public GroundDetector GroundDetector { get; private set; }

        public void Initialize()
        {
            //InitizalizeWallDetector();
            InitizalizeGroundDetector();
        }

        private void InitizalizeGroundDetector()
        {
            GroundDetector = new GroundDetector();
            
            if (leftFoot == null || rightFoot == null)
            {
                Debug.LogError($"Left foot: {leftFoot} / Right foot: {rightFoot}");
                return;
            }
            
            GroundDetector.RegisterCheckPoint(leftFoot, rightFoot, groundLayer);
        }
        /*private void InitizalizeWallDetector()
        {
            WallDetector = new WallDetector();
            
            if (_leftHand == null || _rightHand == null)
            {
                Debug.LogError($"Left foot: {leftFoot} / Right foot: {_rightHand}");
                return;
            }
            
            WallDetector.RegisterCheckPoint(_leftHand, _rightHand, groundLayer);
        }*/
    }
}
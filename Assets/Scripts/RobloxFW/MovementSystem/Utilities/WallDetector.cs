using UnityEngine;

namespace RobloxFW.MovementSystem.Utilities
{
    public class WallDetector
    {
        private Transform _leftHand;
        private Transform _rightHand;
        private LayerMask _groundLayer;
        
        private RaycastHit leftHitInfo;
        private RaycastHit rightHitInfo;
        public void RegisterCheckPoint(Transform leftHand, Transform rightHand, LayerMask groundLayer)
        {
            _leftHand = leftHand;
            _rightHand = rightHand;
            _groundLayer = groundLayer;
        }
        
        public bool IsLeftHandOnWall(float wallCheckDistance)
        {
            Debug.DrawRay(_leftHand.position, _leftHand.forward * wallCheckDistance, Color.red);
            
            var result = Physics.Raycast(
                _leftHand.position,
                _leftHand.forward,
                out leftHitInfo,
                wallCheckDistance,
                _groundLayer);
            
            return result;
        }

        public bool IsRightHandOnWall(float wallCheckDistance)
        {
            Debug.DrawRay(_rightHand.position, _rightHand.forward * wallCheckDistance, Color.red);
            
            var result = Physics.Raycast(
                _rightHand.position,
                _rightHand.forward,
                out rightHitInfo,
                wallCheckDistance,
                _groundLayer);
            
            return result;
        }
        
        public Vector3 GetWallPositionByLeftHand()
        {
            return leftHitInfo.point;
        }

        public Vector3 GetWallPositionByRightHand()
        {
            return rightHitInfo.point;
        }

        public Vector3 GetDirection()
        {
            return - (leftHitInfo.normal + rightHitInfo.normal).normalized;
        }
    }
}
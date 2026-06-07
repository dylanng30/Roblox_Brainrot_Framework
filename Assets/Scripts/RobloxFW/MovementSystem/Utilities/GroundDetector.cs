using UnityEngine;

namespace RobloxFW.MovementSystem.Utilities
{
    public class GroundDetector
    {
        private Transform _leftFoot;
        private Transform _rightFoot;
        private LayerMask _groundLayer;
        
        private RaycastHit leftHitInfo;
        private RaycastHit rightHitInfo;
        
        public void RegisterCheckPoint(Transform leftFoot, Transform rightFoot, LayerMask groundLayer )
        {
            _leftFoot = leftFoot;
            _rightFoot = rightFoot;
            _groundLayer = groundLayer;
        }
        
        public bool IsLeftFootOnGround(float groundCheckDistance)
        {
            Debug.DrawRay(_leftFoot.position, Vector3.down * groundCheckDistance, Color.red);
            
            var result = Physics.Raycast(
                _leftFoot.position,
                Vector3.down,
                out leftHitInfo,
                groundCheckDistance,
                _groundLayer);
            
            return result;
        }

        public bool IsRightFootOnGround(float groundCheckDistance)
        {
            Debug.DrawRay(_rightFoot.position, Vector3.down * groundCheckDistance, Color.red);
            
            var result = Physics.Raycast(
                _rightFoot.position,
                Vector3.down,
                out rightHitInfo,
                groundCheckDistance,
                _groundLayer);
            
            return result;
        }
        
        public Vector3 GetGroundPositionByLeftFoot()
        {
            return leftHitInfo.point;
        }

        public Vector3 GetGroundPositionByRightFoot()
        {
            return rightHitInfo.point;
        }
    }
}
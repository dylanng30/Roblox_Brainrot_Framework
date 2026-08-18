using _BananaSpeed.Movement.States;
using UnityEngine;

namespace _BananaSpeed.Movement
{
    public class BS_EnviromentRaycastDetector : MonoBehaviour, BS_IEnviromentDetector
    {
        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Wall/Climb Detection")]
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private float wallCheckDistance = 0.5f;
        [SerializeField] private LayerMask wallLayer;

        public bool IsGrounded { get; private set; }
        public bool CanClimb { get; private set; }

        private void Update()
        {
            CheckGround();
            CheckWall();
        }

        private void CheckGround()
        {
            Vector3 origin = groundCheckPoint != null ? groundCheckPoint.position : transform.position;
            IsGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
        }

        private void CheckWall()
        {
            Vector3 origin = wallCheckPoint != null ? wallCheckPoint.position : transform.position;
            CanClimb = Physics.Raycast(origin, transform.forward, wallCheckDistance, wallLayer);
        }

        private void OnDrawGizmosSelected()
        {
            // Draw Ground Ray
            Vector3 gOrigin = groundCheckPoint != null ? groundCheckPoint.position : transform.position;
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(gOrigin, gOrigin + Vector3.down * groundCheckDistance);

            // Draw Wall Ray
            Vector3 wOrigin = wallCheckPoint != null ? wallCheckPoint.position : transform.position;
            Gizmos.color = CanClimb ? Color.green : Color.red;
            Gizmos.DrawLine(wOrigin, wOrigin + transform.forward * wallCheckDistance);
        }
    }
}

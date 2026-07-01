using UnityEngine;

namespace RobloxFW.MovementSystem.Player.Components
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform target;
        public Vector3 targetOffset = new Vector3(0.5f, 1.5f, 0f);
        public float baseDistance = 5.0f;
        public Vector2 verticalAngleLimits = new Vector2(-30f, 20f);

        [Header("Rotation Settings")]
        public float rotationSpeed = 50f;

        [Header("Smoothing Settings")]
        public float rotationSmoothTime = 0.05f;
        public float distanceRecoverySmoothTime = 0.15f;

        [Header("Collision Settings")]
        [SerializeField] private LayerMask collisionLayerMask;
        [SerializeField] private float collisionRadius = 0.2f;
        [SerializeField] private float minCollisionDistance = 0.5f;

        public bool IsLocked = false;

        private float currentYaw;
        private float currentPitch;
        private float targetYaw;
        private float targetPitch;

        private float yawSmoothVelocity;
        private float pitchSmoothVelocity;
        private float distanceSmoothVelocity;

        private float currentDistance;
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            if (target == null)
            {
                Debug.LogWarning("PlayerCameraController: Chưa gán target.");
                return;
            }
            
            Vector3 angles = transform.eulerAngles;
            currentYaw = targetYaw = angles.y;
            currentPitch = targetPitch = angles.x;

            currentDistance = baseDistance;
        }

        void Update()
        {
            if (target == null || IsLocked)
                return;

            HandleInput();
        }

        void LateUpdate()
        {   
            if (target == null)
                return;
            
            UpdateCameraLogic();
        }
        
        public void Register(Transform focusedTarget)
        {
            target = focusedTarget;
        }

        #region --- Camera Logic ---

        private void UpdateCameraLogic()
        {
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawSmoothVelocity, rotationSmoothTime);
            currentPitch = Mathf.SmoothDampAngle(currentPitch, targetPitch, ref pitchSmoothVelocity, rotationSmoothTime);

            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);

            Quaternion yawRotation = Quaternion.Euler(0, currentYaw, 0);
            Vector3 pivotPosition = target.position + yawRotation * targetOffset;

            HandleCameraCollision(pivotPosition, rotation);

            Vector3 finalPosition = pivotPosition - rotation * Vector3.forward * currentDistance;
            
            transform.position = finalPosition;
            transform.rotation = rotation;
        }

        private void HandleCameraCollision(Vector3 pivotPosition, Quaternion cameraRotation)
        {
            Vector3 desiredCameraPosition = pivotPosition - cameraRotation * Vector3.forward * baseDistance;
            
            Vector3 direction = desiredCameraPosition - pivotPosition;
            float distanceToDesired = direction.magnitude;
            direction.Normalize();

            float evaluatedTargetDistance = baseDistance;

            if (Physics.SphereCast(pivotPosition, collisionRadius, direction, out RaycastHit hit, distanceToDesired, collisionLayerMask))
            {
                evaluatedTargetDistance = Mathf.Clamp(hit.distance, minCollisionDistance, baseDistance);
            }

            if (evaluatedTargetDistance < currentDistance)
            {
                currentDistance = evaluatedTargetDistance;
                distanceSmoothVelocity = 0f;
            }
            else
            {
                currentDistance = Mathf.SmoothDamp(currentDistance, evaluatedTargetDistance, ref distanceSmoothVelocity, distanceRecoverySmoothTime);
            }
        }

        #endregion


        private void HandleInput()
        {
            HandleMoveByMouse();
            HandleMoveByMobile();
        }
        
        private void HandleMoveByMouse()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            targetYaw += mouseX * rotationSpeed * Time.deltaTime;
            targetPitch -= mouseY * rotationSpeed * Time.deltaTime;
            targetPitch = Mathf.Clamp(targetPitch, verticalAngleLimits.x, verticalAngleLimits.y);
        }

        private void HandleMoveByMobile()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 cameraMovementDirection = touch.deltaPosition;
                    
                    targetYaw += cameraMovementDirection.x * rotationSpeed * Time.deltaTime * 0.2f;
                    targetPitch -= cameraMovementDirection.y * rotationSpeed * Time.deltaTime * 0.2f;
                    targetPitch = Mathf.Clamp(targetPitch, verticalAngleLimits.x, verticalAngleLimits.y);
                }
            }
        }

    }
}
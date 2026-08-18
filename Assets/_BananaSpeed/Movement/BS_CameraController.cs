using DG.Tweening;
using Dylanng;
using RobloxFW.InputSystem;
using UnityEngine;

namespace _BananaSpeed.Movement
{
    public class BS_CameraController : Singleton<BS_CameraController>
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
        public float speed = 5;

        [Header("Collision Settings")]
        [SerializeField] private LayerMask collisionLayerMask;
        [SerializeField] private float collisionRadius = 0.2f;
        [SerializeField] private float minCollisionDistance = 0.5f;

        [Header("Camera Shake Settings")]
        [SerializeField] private float shakeSpeed = 25f;
        [SerializeField] private float lightShakeStrength = 0.1f;
        [SerializeField] private float lightShakeDuration = 0.2f;
        [SerializeField] private float mediumShakeStrength = 0.3f;
        [SerializeField] private float mediumShakeDuration = 0.35f;
        [SerializeField] private float heavyShakeStrength = 0.6f;
        [SerializeField] private float heavyShakeDuration = 0.5f;

        public bool IsLocked = false;

        private float currentYaw;
        private float currentPitch;
        private float targetYaw;
        private float targetPitch;

        private float yawSmoothVelocity;
        private float pitchSmoothVelocity;
        private float distanceSmoothVelocity;

        private float currentDistance;

        private IPlayerInput _playerInput;

        // Shake
        private float currentShakeIntensity = 0f;
        private Tween shakeTween;

        void Start()
        {
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

        public void SetupInput(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        void Update()
        {
            if (target == null || IsLocked)
                return;

            if (_playerInput == null)
            {
                _playerInput = ServiceLocator.Get<GameInputManager>();
            }

            HandleInput();
        }

        void LateUpdate()
        {   
            if (target == null)
                return;
            
            UpdateCameraLogic();
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
            
            Vector3 shakeOffset = GetShakeOffset();
            
            transform.rotation = rotation;
            transform.position = finalPosition + shakeOffset;
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
            if (_playerInput == null) return;

            Vector2 lookInput = _playerInput.Look;
            
            if (lookInput.sqrMagnitude > 0)
            {
                targetYaw += lookInput.x * rotationSpeed * Time.deltaTime;
                targetPitch -= lookInput.y * rotationSpeed * Time.deltaTime;
                targetPitch = Mathf.Clamp(targetPitch, verticalAngleLimits.x, verticalAngleLimits.y);
            }
        }

        public void ResetFaceDirection()
        {
            if (target == null) return;
            
            targetYaw = target.eulerAngles.y;
            currentYaw = targetYaw;
            
            targetPitch = 10f; 
            currentPitch = targetPitch;
            
            yawSmoothVelocity = 0f;
            pitchSmoothVelocity = 0f;
            distanceSmoothVelocity = 0f;

            currentDistance = baseDistance;
            
            UpdateCameraLogic();
        }

        #region --- Camera Shake ---

        public void DoLightShake()
        {
            ApplyShake(lightShakeStrength, lightShakeDuration);
        }

        public void DoMediumShake()
        {
            ApplyShake(mediumShakeStrength, mediumShakeDuration);
        }

        public void DoHeavyShake()
        {
            ApplyShake(heavyShakeStrength, heavyShakeDuration);
        }

        private void ApplyShake(float strength, float duration)
        {
            shakeTween?.Kill();

            currentShakeIntensity = strength;
            
            shakeTween = DOTween.To(() => currentShakeIntensity, x => currentShakeIntensity = x, 0f, duration)
                .SetEase(Ease.OutQuad);
        }

        private Vector3 GetShakeOffset()
        {
            if (currentShakeIntensity <= 0f) return Vector3.zero;
            
            float time = Time.time * shakeSpeed;
            float offsetX = (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f;
            float offsetY = (Mathf.PerlinNoise(0f, time) - 0.5f) * 2f;
            float offsetZ = (Mathf.PerlinNoise(time, time) - 0.5f) * 2f;

            return new Vector3(offsetX, offsetY, offsetZ) * currentShakeIntensity;
        }

        #endregion
    }
}
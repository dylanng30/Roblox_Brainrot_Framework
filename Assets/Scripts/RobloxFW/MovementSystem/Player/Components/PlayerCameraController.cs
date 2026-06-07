using UnityEngine;

namespace RobloxFW.MovementSystem.Player.Components
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform target;
        public float baseDistance = 5.0f;
        public Vector2 verticalAngleLimits = new Vector2(-30f, 80f);

        [Header("Rotation Settings")]
        public float rotationSpeed = 50f;
        public float smoothSpeed = 10f;
        public float zoomSpeed = 2f;
        public Vector2 zoomLimits = new Vector2(2f, 15f);
        
        [Header("Movement Settings")]
        [SerializeField] private float offsetScale;
        public bool IsLocked = false;
        
        [Header("Other Settings")]
        [SerializeField] private LayerMask layer;

        private float currentYaw;
        private float currentPitch;
        private float targetYaw;
        private float targetPitch;
        private Vector3 lastMousePosition;
        private bool isDragging = false;
        private float currentDistance;

        public void Register(Transform focusedTarget)
        {
            target = focusedTarget;
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            if (target == null)
            {
                Debug.LogWarning("OrbitCamera: Chưa gán target");
                return;
            }
            
            Vector3 angles = transform.eulerAngles;
            currentYaw = targetYaw = angles.y;
            currentPitch = targetPitch = angles.x;
        }

        void Update()
        {
            if (target == null)
                return;

            HandleInput();
        }

        void LateUpdate()
        {   
            // Lerp mượt
            if (target == null)
                return;
            
            currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * smoothSpeed);
            currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * smoothSpeed);

            // Tính rotation & vị trí
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            currentDistance = CalculateDistance();
            
            Vector3 targetPosition = target.position - rotation * Vector3.forward * currentDistance + Vector3.up * 3;

            
            //Đang xử lý:
            //Viết hàm tính vị trí của camera khi vướng vật cản
            // test offSet
            // Bị giật khi camera trong vùng phát hiện raycast 2 bên
            
            //Test Offset
            /*Vector3 offSetVector = CalculateOffset() *  offsetScale;
            targetPosition += offSetVector;*/
            //
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
            transform.LookAt(target);
        }

        private float CalculateDistance()
        {
            //PlayerMovement -> Camera
            Vector3 direction = transform.position - target.position;
            direction.Normalize();
            
            if (Physics.Raycast(target.position, direction, out RaycastHit hit, baseDistance, layer))
            {
                return hit.distance;
            }

            return baseDistance;
            
            //New solution (test)
            /*Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            Vector3 direction = targetRotation * -Vector3.forward; // Hướng từ Target lùi về Camera

            // Sử dụng SphereCast thay vì Raycast
            // Tham số: Gốc, Bán kính cầu, Hướng, Out Hit, Khoảng cách max, LayerMask
            if (Physics.SphereCast(target.position, cameraRadius, direction, out RaycastHit hit, baseDistance, collisionLayers))
            {
                return hit.distance;
            }

            return baseDistance;*/
        }

        private Vector3 CalculateOffset()
        {
            if (Physics.Raycast(transform.position, -transform.right, 2f))
            {
                Debug.Log("Trai");
                return transform.right;
            }
            if (Physics.Raycast(transform.position, transform.right, 2f))
            {
                Debug.Log("Phai");
                return -transform.right;
            }
            
            return Vector3.zero;
        }

        #region ---HandleInput---

        private void HandleInput()
        {
            HandleMoveByMouse();
            HandleMoveByMobile();
            //HandleZoomByPC();
            //HandleZoomByMobile();
        }
        
        // --- Move bằng chuột (PC)
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
                Vector2 cameraMovementDirection = touch.position - touch.deltaPosition;
                cameraMovementDirection.Normalize();
                
                targetYaw += cameraMovementDirection.x * rotationSpeed * Time.deltaTime;
                targetPitch -= cameraMovementDirection.y * rotationSpeed * Time.deltaTime;
                targetPitch = Mathf.Clamp(targetPitch, verticalAngleLimits.x, verticalAngleLimits.y);
                
            }
        }
        
        
        // --- Zoom bằng chuột (PC) ---
        private void HandleZoomByPC()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                baseDistance = Mathf.Clamp(baseDistance - scroll * 5f, zoomLimits.x, zoomLimits.y);
            }
        }
        // --- Zoom bằng cảm ứng (Mobile) ---
        private void HandleZoomByMobile()
        {
            if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                
                Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
                Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

                float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
                float currentMagnitude = (touch0.position - touch1.position).magnitude;

                float diff = currentMagnitude - prevMagnitude;

                baseDistance = Mathf.Clamp(baseDistance - diff * zoomSpeed * Time.deltaTime * 0.1f, zoomLimits.x, zoomLimits.y);
            }
        }
        #endregion
        
    }
}
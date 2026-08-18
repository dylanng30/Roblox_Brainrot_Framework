using Dylanng;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RobloxFW.InputSystem
{
    public class GameInputManager : ManagerBase, IPlayerInput, IUpdatable
    {
        [Header("--- MOBILE SETTINGS ---")]
        public MobileActionButton jumpButton;
        public VirtualJoystick virtualJoystick;
        public bool useLeftScreenForMovement = true;
        public float mobileLookSensitivity = 0.1f;
        
        [Header("--- EDITOR SETTINGS ---")]
        public bool enableDebugLogs = true;

        public Vector2 Movement { get; private set; }
        public Vector2 Look { get; private set; }
        
        public bool IsPressingSpace { get; private set; }
        public bool IsPressingLeftControl { get; private set; }
        public bool IsPressingRightControl { get; private set; }
        public bool Dash { get; private set; }
        public bool Sprint { get; private set; }
        public int HotBarIndex { get; private set; }

        private int _cameraTouchId = -1;
        private int _movementTouchId = -1;
        private Vector2 _movementTouchStart;

        private bool _isMouseLooking = false;
        private bool _isMouseMoving = false;
        private Vector2 _mouseMovementStart;
        private Vector2 _lastMousePosition;

        public override void Initialize()
        {
            base.Initialize();

            jumpButton.HoldAction += HandleJumpButtonAction;

            ServiceLocator.Get<ITickSystem>()?.Register(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            jumpButton.HoldAction -= HandleJumpButtonAction;
            ServiceLocator.Get<ITickSystem>()?.Unregister(this);
        }      

        public void OnUpdate(float deltaTime)
        {
            Look = Vector2.zero;

            ProcessPCInput();
            ProcessMobileInput();

            _lastMousePosition = Input.mousePosition;

            IsPressingSpace = Input.GetKey(KeyCode.Space) || _isJumpButtonHeld;
        }

        private bool _isJumpButtonHeld;

        private void HandleJumpButtonAction(bool isHold)
        {
            _isJumpButtonHeld = isHold;
        }

        private void ProcessPCInput()
        {
            Vector2 pcMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (pcMovement.sqrMagnitude > 0)
            {
                Movement = pcMovement;
            }
            else if (_movementTouchId == -1 && !_isMouseMoving)
            {
                Movement = Vector2.zero;
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsPointerOverUI(-1))
                {
                    if (Input.mousePosition.x > Screen.width / 2f)
                    {
                        _isMouseLooking = true;
                        if (enableDebugLogs) Debug.Log("[InputSystem] Editor: Started Camera Look (Mouse Drag)");
                    }
                    else if (Input.mousePosition.x <= Screen.width / 2f && useLeftScreenForMovement)
                    {
                        _isMouseMoving = true;
                        _mouseMovementStart = Input.mousePosition;
                        if (virtualJoystick != null) virtualJoystick.Show(_mouseMovementStart);
                        if (enableDebugLogs) Debug.Log("[InputSystem] Editor: Started Movement (Mouse Drag)");
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (_isMouseLooking)
                {
                    Vector2 mouseDelta = (Vector2)Input.mousePosition - _lastMousePosition;
                    Look = mouseDelta * mobileLookSensitivity;
                    if (Look.sqrMagnitude > 0)
                    {
                        if (enableDebugLogs) Debug.Log($"[InputSystem] Editor Look Delta: {Look}");
                    }
                }
                else if (_isMouseMoving)
                {
                    Vector2 delta = (Vector2)Input.mousePosition - _mouseMovementStart;
                    float maxRadius = virtualJoystick != null ? virtualJoystick.GetRadius() : Screen.width * 0.1f;
                    Vector2 dir = delta / maxRadius;
                    if (dir.sqrMagnitude > 1) dir.Normalize();
                    Movement = dir;
                    if (virtualJoystick != null) virtualJoystick.UpdateHandle(Movement);
                    if (enableDebugLogs) Debug.Log($"[InputSystem] Editor Movement Drag: {Movement}");
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_isMouseLooking)
                {
                    _isMouseLooking = false;
                    if (enableDebugLogs) Debug.Log("[InputSystem] Editor: Ended Camera Look");
                }
                if (_isMouseMoving)
                {
                    _isMouseMoving = false;
                    Movement = Vector2.zero;
                    if (virtualJoystick != null) virtualJoystick.Hide();
                    if (enableDebugLogs) Debug.Log("[InputSystem] Editor: Ended Movement");
                }
            }
#endif
        }

        private void ProcessMobileInput()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (IsPointerOverUI(touch.fingerId) && touch.phase == TouchPhase.Began)
                {
                    continue;
                }

                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x > Screen.width / 2f && _cameraTouchId == -1)
                    {
                        _cameraTouchId = touch.fingerId;
                        if (enableDebugLogs) Debug.Log($"[InputSystem] Mobile: Started Camera Look (FingerId: {_cameraTouchId})");
                    }
                    else if (touch.position.x <= Screen.width / 2f && _movementTouchId == -1 && useLeftScreenForMovement)
                    {
                        _movementTouchId = touch.fingerId;
                        _movementTouchStart = touch.position;
                        if (virtualJoystick != null) virtualJoystick.Show(_movementTouchStart);
                        if (enableDebugLogs) Debug.Log($"[InputSystem] Mobile: Started Movement Screen (FingerId: {_movementTouchId})");
                    }
                }
                
                if (touch.fingerId == _cameraTouchId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Look = touch.deltaPosition * mobileLookSensitivity;
                        if (enableDebugLogs) Debug.Log($"[InputSystem] Look Delta: {Look}");
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        _cameraTouchId = -1;
                        if (enableDebugLogs) Debug.Log("[InputSystem] Ended Camera Look");
                    }
                }
                else if (touch.fingerId == _movementTouchId && useLeftScreenForMovement)
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        Vector2 delta = touch.position - _movementTouchStart;
                        float maxRadius = virtualJoystick != null ? virtualJoystick.GetRadius() : Screen.width * 0.1f;
                        Vector2 dir = delta / maxRadius;
                        if (dir.sqrMagnitude > 1) dir.Normalize();
                        Movement = dir;
                        if (virtualJoystick != null) virtualJoystick.UpdateHandle(Movement);
                        if (enableDebugLogs) Debug.Log($"[InputSystem] Movement from Left Screen: {Movement}");
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        _movementTouchId = -1;
                        Movement = Vector2.zero;
                        if (virtualJoystick != null) virtualJoystick.Hide();
                        if (enableDebugLogs) Debug.Log("[InputSystem] Ended Movement Screen");
                    }
                }
            }
        }

        private bool IsPointerOverUI(int fingerId)
        {
            if (EventSystem.current == null) return false;
            return EventSystem.current.IsPointerOverGameObject(fingerId);
        }

        public bool PressLeftMouse() => Input.GetMouseButtonDown(0);
        public bool IsDraggingLeftMouse() => Input.GetMouseButton(0);
        public bool DropLeftMouse() => Input.GetMouseButtonUp(0);
        
    }
}

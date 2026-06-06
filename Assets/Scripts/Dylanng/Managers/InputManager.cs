using UnityEngine;
using Dylanng.Core.Base;
using Dylanng.Core;
using Dylanng.Core.Systems.TickSystem;

namespace Dylanng.Services
{
    public class InputManager : ManagerBase, IUpdatable
    {
        public Vector2 MovementInput { get; private set; }
        public Vector2 RotationInput { get; private set; }
        public float ZoomInput { get; private set; }

        public bool JumpInput { get; private set; }
        public bool SprintInput { get; private set; }
        public bool AttackInput { get; private set; }
        public bool ComboInput { get; private set; }
        public bool BlockInput { get; private set; }
        public bool RunInput { get; private set; }

        public bool UsePCInput { get; set; } = true;

        public override void Initialize()
        {
            ServiceLocator.Register<InputManager>(this);
            ServiceLocator.Get<ITickSystem>().Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            if (UsePCInput)
            {
                PollPCKeyboardInput();
            }
            else
            {
                PollMobileGestures();
            }
        }

        private void PollPCKeyboardInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            MovementInput = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);

            JumpInput = Input.GetButton("Jump");
            SprintInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            AttackInput = Input.GetButton("Fire1");
            BlockInput = Input.GetButton("Fire2");

            RotationInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            ZoomInput = Input.GetAxis("Mouse ScrollWheel");
        }

        private void PollMobileGestures()
        {
            ZoomInput = 0f;
            if (Input.touchCount >= 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                float prevMagnitude = (touch1PrevPos - touch2PrevPos).magnitude;
                float currentMagnitude = (touch1.position - touch2.position).magnitude;

                ZoomInput = currentMagnitude - prevMagnitude;
            }
        }

        //UI Mobile
        public void SetMovementInput(Vector2 input) => MovementInput = input;
        public void SetRotationInput(Vector2 input) => RotationInput = input;
        public void SetZoomInput(float input) => ZoomInput = input;

        public void SetJumpInput(bool isPressed) => JumpInput = isPressed;
        public void SetSprintInput(bool isPressed) => SprintInput = isPressed;
        public void SetAttackInput(bool isPressed) => AttackInput = isPressed;
        public void SetBlockInput(bool isPressed) => BlockInput = isPressed;
        public void SetComboInput(bool isPressed) => ComboInput = isPressed;
        public void SetRunInput(bool isPressed) => RunInput = isPressed;
    }
}
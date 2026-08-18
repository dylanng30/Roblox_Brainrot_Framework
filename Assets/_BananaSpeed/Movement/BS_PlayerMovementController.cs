using _BananaSpeed.Movement.Data;
using _BananaSpeed.Movement.States;
using Dylanng;
using RobloxFW.InputSystem;
using UnityEngine;

namespace _BananaSpeed.Movement
{
    public class BS_PlayerMovementController : MonoBehaviour, BS_IMovable
    {
        [Header("--- UNITY COMPONENTS ---")]
        [SerializeField] private Rigidbody rb;
        
        [Header("--- REFERENCES ---")]
        [SerializeField] private BS_AnimationController animController;
        [SerializeField] private BS_EnviromentRaycastDetector raycastDetector;
        [SerializeField] private BS_PlayerInputAdapter playerInput;
        
        [Header("--- DATA ---")]
        [SerializeField] private BS_CharacterSO characterData;
        
        private BS_PlayerMovementStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine =
                new BS_PlayerMovementStateMachine(this, playerInput, raycastDetector, characterData, animController);
            Debug.Log("[BS_DEBUG] StateMachine Created");
        }

        private void Start()
        {
            _stateMachine.Initialize(_stateMachine.IdleState);
            Debug.Log("[BS_DEBUG] StateMachine Initialized with IdleState");
        }

        private void Update()
        {
            _stateMachine.HandleInput();
            _stateMachine.Update();

            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log($"[BS_DEBUG] Input: {playerInput.MovementVector} | IsGrounded: {_stateMachine.IsGrounded()} | Speed: {rb.velocity}");
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.PhysicsUpdate();
        }

        // Movement
        public void SetHorizontalVelocity(Vector3 velocity) 
            => rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        public void SetVerticalVelocity(float yVelocity) 
            => rb.velocity = new Vector3(rb.velocity.x, yVelocity, rb.velocity.z);
        public float GetVerticalVelocity() 
            => rb.velocity.y;
        public float GetCurrentYAngle() 
            => rb.rotation.eulerAngles.y;
        public void AddImpulse(Vector3 force) 
            => rb.AddForce(force, ForceMode.VelocityChange);
        public void AddAcceleration(Vector3 force) 
            => rb.AddForce(force, ForceMode.Acceleration);
        public void SetRotation(float yAngle) 
            => rb.MoveRotation(Quaternion.Euler(0, yAngle, 0));
    }
}
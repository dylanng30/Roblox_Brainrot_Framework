using Dylanng.Core.Systems.TickSystem;
using RobloxFW.MovementSystem.Data.Player.SO;
using RobloxFW.MovementSystem.Data.Player;
using RobloxFW.MovementSystem.Player;
using RobloxFW.MovementSystem.Utilities;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.Components
{
    public class PlayerMovementController : MonoBehaviour, IUpdatable, IFixedUpdatable
    {
        [field: SerializeField] public PlayerMovementSO Data { get; private set; }
        [field: SerializeField] public PlayerAnimationData  AnimationData { get; private set; }
        [field: SerializeField] public PlayerCameraController CameraController { get; private set; }
        [field: SerializeField] public AnimationController AnimationController { get; private set; }
        
        public Animator Animator { get; private set; }
        public Rigidbody RigidBody { get; private set; }
        public PlayerInput PlayerInput {get; private set;}
        public PlayerColliderDetectors ColliderDetectors { get; private set; }
        
        private PlayerMovementStateMachine _movementStateMachine;
        
        void Awake()
        {
            LoadComponents();
            _movementStateMachine = new PlayerMovementStateMachine(this);

            AnimationController.OnAnimEnter += OnMovementStateAnimationEnterEvent;
            AnimationController.OnAnimTransition += OnMovementStateAnimationTransitionEvent;
            AnimationController.OnAnimExit += OnMovementStateAnimationExitEvent;
        }

        private void Start()
        {
            Initialize();
        }
        
        public void OnUpdate(float deltaTime)
        {
            _movementStateMachine.HandleInput();
            _movementStateMachine.Update();
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            _movementStateMachine.PhysicsUpdate();
        }

        public void OnMovementStateAnimationEnterEvent()
        {
            _movementStateMachine.OnAnimationEnterEvent();
        }

        public void OnMovementStateAnimationExitEvent()
        {
            _movementStateMachine.OnAnimationExitEvent();
        }

        public void OnMovementStateAnimationTransitionEvent()
        {
            _movementStateMachine.OnAnimationTransitionEvent();
        }

        #region ---INITIALIZE---

        private void Initialize()
        {
            InitializePlayerAnimationData();
            InitializeColliderDetectors();
        }

        private void InitializePlayerAnimationData()
        {
            AnimationData.Initialize();
        }

        private void InitializeColliderDetectors()
        {
            ColliderDetectors.Initialize();
        }

        #endregion

        #region ---LOAD---

        private void LoadComponents()
        {
            LoadPlayerInput();
            LoadRigidBody();
            LoadAnimator();
            LoadColliderDetectors();
        }
        private void LoadPlayerInput()
        {
            if (PlayerInput != null)
            {
                return;
            }

            PlayerInput = GetComponent<PlayerInput>();
        }

        private void LoadRigidBody()
        {
            if(RigidBody != null)
            {
                return;
            }
            
            RigidBody = GetComponent<Rigidbody>();
        }
        private void LoadAnimator()
        {
            if (Animator != null)
            {
                return;
            }
            
            Animator = GetComponent<Animator>();
        }

        private void LoadColliderDetectors()
        {
            if (ColliderDetectors != null)
            {
                return;
            }
            
            ColliderDetectors = GetComponentInChildren<PlayerColliderDetectors>();
        }

        #endregion
    }
}
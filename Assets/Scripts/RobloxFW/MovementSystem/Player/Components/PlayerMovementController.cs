using System;
using Dylanng.Core;
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
        public PlayerMovementSO Data;
        public PlayerAnimationData AnimationData;
        public PlayerCameraController CameraController;
        public AnimationController AnimationController;
        public PlayerColliderDetectors ColliderDetectors;
        public Animator Animator;
        public Rigidbody RigidBody;
        public PlayerInput PlayerInput;
        
        private PlayerMovementStateMachine _movementStateMachine;
        
        void Awake()
        {
            _movementStateMachine = new PlayerMovementStateMachine(this);

            AnimationController.OnAnimEnter += OnMovementStateAnimationEnterEvent;
            AnimationController.OnAnimTransition += OnMovementStateAnimationTransitionEvent;
            AnimationController.OnAnimExit += OnMovementStateAnimationExitEvent;
        }

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            //ServiceLocator.Get<ITickSystem>()?.Register(this);
        }

        private void OnDisable()
        {
            //ServiceLocator.Get<ITickSystem>()?.Unregister(this);
        }

        private void Update()
        {
            OnUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            OnFixedUpdate(Time.fixedDeltaTime);
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
    }
}
using _BananaSpeed.Movement.Data;
using _BananaSpeed.Movement.Interfaces;
using _BananaSpeed.Movement.States.Airborne;
using _BananaSpeed.Movement.States.Grounded;
using Dylanng;
using RobloxFW.MovementSystem;
using UnityEngine;

namespace _BananaSpeed.Movement.States
{
    public class BS_PlayerMovementStateMachine : BaseMovementStateMachine
    {
        // Data
        public BS_CharacterSO Data { get; }
        public BS_PlayerReusableData SharedData { get; }
        
        // Interfaces
        public BS_IMovable Movement { get; }
        public BS_IPlayerInput Input { get; }
        public BS_IEnviromentDetector Detector { get; }
        public BS_IAnimationController Animation { get; }
        
        public Transform CameraTransform { get; }

        // States
        public BS_PlayerIdleState IdleState { get; }
        public BS_PlayerRunState RunState { get; }
        public BS_PlayerJumpState JumpState { get; }
        public BS_PlayerSecondJumpState SecondJumpState { get; }
        public BS_PlayerFallState FallState { get; }
        //

        public BS_PlayerMovementStateMachine(
            BS_IMovable movement,
            BS_IPlayerInput input,
            BS_IEnviromentDetector detector,
            BS_CharacterSO data,
            BS_IAnimationController animation)
        {
            Movement = movement;
            Input = input;
            Detector = detector;
            Data = data;
            Animation = animation;
            
            CameraTransform = Camera.main.transform;

            Data.AnimationData.Initialize();

            Animation.OnAnimationEnterEvent += OnAnimationEnterEvent;
            Animation.OnAnimationExitEvent += OnAnimationExitEvent;
            Animation.OnAnimationTransitionEvent += OnAnimationTransitionEvent;

            SharedData = new BS_PlayerReusableData();

            IdleState = new BS_PlayerIdleState(this);
            RunState = new BS_PlayerRunState(this);
            JumpState = new BS_PlayerJumpState(this);
            SecondJumpState = new BS_PlayerSecondJumpState(this);
            FallState = new BS_PlayerFallState(this);
        }

        public bool IsGrounded()
        {
            if (Detector == null)
            {
                GameLogger.LogError("[BS_PlayerMovementStateMachine] Detector is null");
                return false;
            }
            return Detector.IsGrounded;
        }

        public bool CanClimb()
        {
            if (Detector == null)
            {
                GameLogger.LogError("[BS_PlayerMovementStateMachine] Detector is null");
                return false;
            }
            return Detector.CanClimb;
        }

        public override void CleanUp()
        {
            base.CleanUp();
            if (Animation != null)
            {
                Animation.OnAnimationEnterEvent -= OnAnimationEnterEvent;
                Animation.OnAnimationExitEvent -= OnAnimationExitEvent;
                Animation.OnAnimationTransitionEvent -= OnAnimationTransitionEvent;
            }
        }
    }
}


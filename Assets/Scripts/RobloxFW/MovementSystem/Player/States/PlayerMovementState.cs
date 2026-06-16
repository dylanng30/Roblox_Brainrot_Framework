using Dylanng.Core.State;
using RobloxFW.MovementSystem.Data.Player.States.Airborne;
using RobloxFW.MovementSystem.Data.Player.States.Grounded;
using RobloxFW.MovementSystem.Player.Components;
using RobloxFW.MovementSystem.Player;
using RobloxFW.MovementSystem.Utilities;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States
{
    public class PlayerMovementState : StateBase
    {
        protected PlayerMovementStateMachine _stateMachine;
        
        protected Vector2 movementInput;

        protected bool shouldWalk;
        
        protected PlayerGroundedData _movementData;
        protected PlayerAirborneData _airborneData;
        
        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            _stateMachine = playerMovementStateMachine;

            _movementData = _stateMachine.PlayerMovement.Data.GroundedData;
            _airborneData = _stateMachine.PlayerMovement.Data.AirborneData;

            InitializeData();
        }
        
        public override void Enter()
        {
            Debug.Log($"[PlayerMovementState] {GetType().Name}");
        }

        public override void Exit()
        {

        }

        public override void HandleInput()
        {
            ReadMovementInput();
        }

        public override void Update()
        {
            ToggleWalk();
        }

        public override void PhysicsUpdate()
        {

        }

        public override void OnAnimationEnterEvent()
        {
            
        }

        public override void OnAnimationExitEvent()
        {
            
        }

        public override void OnAnimationTransitionEvent()
        {
            
        }
        
        protected override void StartAnimation(int animationHash)
        {
            _stateMachine.PlayerMovement.Animator.SetBool(animationHash, true);
        }

        protected override void StopAnimation(int animationHash)
        {
            _stateMachine.PlayerMovement.Animator.SetBool(animationHash, false);
        }

        #region ---Main Methods---

        private void InitializeData()
        {
            _stateMachine.ReusableData.TimeToReachTargetRotation = _movementData.BaseRotationData.TargetRotationReachTime;
        }

        private void ReadMovementInput()
        {
            movementInput = _stateMachine.PlayerMovement.PlayerInput.Movement;
            //Debug.Log(movementInput);
        }

        protected virtual void Move()
        {
            
        }

        protected virtual void Rotate()
        {
            
        }

        protected virtual bool IsGrounded()
        {
            //Debug.Log($"Checking Grounded");
            var groundDetector = _stateMachine.PlayerMovement.ColliderDetectors.GroundDetector;
            
            if (groundDetector == null)
            {
                Debug.LogError($"[PLAYER MOVEMENT STATE] No ground detector found");
                return false;
            }

            var groundCheckDistance = _airborneData.GroundCheckDistance;
            
            return groundDetector.IsLeftFootOnGround(groundCheckDistance) ||
                   groundDetector.IsRightFootOnGround(groundCheckDistance);
        }

        /*protected virtual bool CanClimb()
        {
            //Debug.Log($"Checking Climbing");
            var wallDetector = _stateMachine.PlayerMovement.ColliderDetectors.WallDetector;

            if (wallDetector == null)
            {
                Debug.LogError($"[PLAYER MOVEMENT STATE] No ground detector found");
                return false;
            }
            
            var wallCheckDistance = _airborneData.WallCheckDistance;
            
            return wallDetector.IsLeftHandOnWall(wallCheckDistance) &&
                   wallDetector.IsRightHandOnWall(wallCheckDistance);
        }*/

        
        #endregion
        
        #region --- Helper Methods---

        
        protected virtual Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = _stateMachine.PlayerMovement.RigidBody.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }
        protected virtual float GetMovementSpeed()
        {
            return _movementData.BaseSpeed *  _stateMachine.ReusableData.MovementSpeedModifier;
        }
        protected virtual Vector3 GetMovementInputDirection()
        {
            var camera = _stateMachine.PlayerMovement.CameraController;
            Vector3 movementDirection = camera.transform.forward * movementInput.y +  camera.transform.right * movementInput.x;
            movementDirection.y = 0f;
            return movementDirection;
        }

        protected virtual void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            _stateMachine.PlayerMovement.RigidBody.AddForce(-playerHorizontalVelocity * _stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected virtual bool IsMovingHorizontally(float miniumMagnitude = 0.1f)
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);
            
            return playerHorizontalMovement.magnitude > miniumMagnitude;
        }

        protected virtual void ResetVelocity()
        {
            _stateMachine.PlayerMovement.RigidBody.velocity = Vector3.zero;
        }

        protected virtual void ToggleWalk()
        {
            shouldWalk = _stateMachine.PlayerMovement.PlayerInput.IsPressingLeftControl ?  true : false;
        }

        #endregion
    }
}
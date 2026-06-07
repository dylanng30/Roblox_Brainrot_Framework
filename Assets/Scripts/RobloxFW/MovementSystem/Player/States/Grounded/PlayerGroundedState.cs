using RobloxFW.MovementSystem.Player.Components;
using RobloxFW.MovementSystem.Player.States;
using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded
{
    public class PlayerGroundedState : PlayerMovementState
    {
        protected PlayerMovementStateMachine _stateMachine;
        
        public PlayerGroundedState(PlayerMovementStateMachine playerStateMachine) : base(playerStateMachine)
        {
            _stateMachine = playerStateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.GroundedParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Move();
            Rotate();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.GroundedParameterHash);
        }

        #region ---METHODS ---
        protected virtual void HandleChangeStateInput()
        {
            if (!IsGrounded())
            {
                _stateMachine.TransitionTo(_stateMachine.FallingState);
                return;
            }

            if (_stateMachine.PlayerMovement.PlayerInput.IsPressingSpace)
            {
                _stateMachine.TransitionTo(_stateMachine.JumpingState);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                _stateMachine.TransitionTo(_stateMachine.MeleeDiagonalState);
            }
        }

        protected override void Move()
        {
            if (movementInput == Vector2.zero || _stateMachine.ReusableData.MovementSpeedModifier == 0f)
            {
                return;
            }

            Vector3 movementDirection = GetMovementInputDirection();
            float movementSpeed = GetMovementSpeed();
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            Vector3 direction = movementDirection * movementSpeed - currentPlayerHorizontalVelocity;
            direction.y = 0;
            
            _stateMachine.PlayerMovement.RigidBody.AddForce(
                direction,
                ForceMode.VelocityChange
            );
        }

        protected override void Rotate()
        {
            if (movementInput == Vector2.zero || _stateMachine.ReusableData.MovementSpeedModifier == 0f)
            {
                return;
            }

            var player = _stateMachine.PlayerMovement;
            var cameraTransform = _stateMachine.PlayerMovement.CameraController.transform;

            Vector3 moveDirection = GetMovementInputDirection();
            moveDirection.y = 0f;
            moveDirection.Normalize();
            
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetRotation,
                _movementData.BaseSpeed *  Time.deltaTime
            );
        }
        #endregion
    }
}
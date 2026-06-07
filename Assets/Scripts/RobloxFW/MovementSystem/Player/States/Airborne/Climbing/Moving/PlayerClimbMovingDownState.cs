using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Airborne.Climbing.Moving
{
    public class PlayerClimbMovingDownState : PlayerClimbMovingState
    {
        public PlayerClimbMovingDownState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            
            //StartAnimation
        }

        public override void Exit()
        {
            base.Exit();
            //StopAnimation
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            var player = _stateMachine.PlayerMovement;
            player.RigidBody.AddForce(-player.transform.up * GetMovementSpeed());
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }
        
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            
            if (IsGrounded())
            {
                _stateMachine.TransitionTo(_stateMachine.LandingState);
                return;
            }
            
            _stateMachine.TransitionTo(_stateMachine.ClimbIdlingState);
        }
        
        protected override void HandleChangeStateInput()
        {
            if (movementInput == Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbIdlingState);
                return;
            }
            
            if (movementInput.x > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbMovingRightState);
            }
            else if (movementInput.y > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbMovingUpState);
            }
            else if (movementInput.x < 0)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbMovingLeftState);
            }
        }
    }
}
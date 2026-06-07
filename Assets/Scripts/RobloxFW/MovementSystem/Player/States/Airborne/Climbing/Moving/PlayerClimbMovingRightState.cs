using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Airborne.Climbing.Moving
{
    public class PlayerClimbMovingRightState : PlayerClimbMovingState
    {
        public PlayerClimbMovingRightState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
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
            //base.PhysicsUpdate();
            var player = _stateMachine.PlayerMovement;
            Vector3 direction = player.transform.right + player.transform.forward;
            player.RigidBody.AddForce(direction * GetMovementSpeed());
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.ClimbIdlingState);
        }
        
        protected override void HandleChangeStateInput()
        {
            if (movementInput == Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbIdlingState);
            }
            
            if (movementInput.x < 0)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbMovingLeftState);
            }
            else if (movementInput.y > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbMovingUpState);
            }
            else if (movementInput.y < 0)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbMovingDownState);
            }
            
            
        }
    }
}
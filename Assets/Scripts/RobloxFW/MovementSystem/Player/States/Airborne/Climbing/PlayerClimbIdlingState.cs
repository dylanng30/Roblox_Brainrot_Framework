using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Airborne.Climbing
{
    public class PlayerClimbIdlingState : PlayerClimbState
    {
        public PlayerClimbIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            HandleChangeStateInput();
        }

        public override void Update()
        {
            base.Update();
            ResetVelocity();
        }

        protected override void HandleChangeStateInput()
        {
            if (movementInput.x > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.ClimbMovingRightState);
            }
            else if (movementInput.x < 0)
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
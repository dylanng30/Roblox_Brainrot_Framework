using RobloxFW.MovementSystem.Player;
using System.Buffers.Text;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.IdleParameterHash);
            
            //ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.IdleParameterHash);
        }
        public override void HandleInput()
        {
            base.HandleInput();
            HandleChangeStateInput();
        }
        
        #region  ---MAIN METHODS---

        protected override void HandleChangeStateInput()
        {
            base.HandleChangeStateInput();
            
            if (movementInput == Vector2.zero)
            {
                return;
            }
            
            if  (shouldWalk)
            {
                _stateMachine.TransitionTo(_stateMachine.WalkingState);
            }
            else
            {
                _stateMachine.TransitionTo(_stateMachine.RunningState);
            }
        }

        #endregion
    }
}
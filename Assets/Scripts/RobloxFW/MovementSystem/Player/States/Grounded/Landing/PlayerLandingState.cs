using RobloxFW.MovementSystem.Player.States;
using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Landing
{
    public class PlayerLandingState : PlayerMovementState
    {
        public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            ResetVelocity();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.LandingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.LandingParameterHash);
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
    }
}
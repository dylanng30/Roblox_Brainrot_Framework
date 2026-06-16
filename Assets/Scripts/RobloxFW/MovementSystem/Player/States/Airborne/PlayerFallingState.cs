using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Airborne
{
    public class PlayerFallingState : PlayerAirborneState
    {
        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.PlayerMovement.RigidBody.useGravity = true;
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.FallParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.FallParameterHash);
        }

        public override void Update()
        {
            base.Update();
            
            if (IsGrounded())
            {
                Debug.Log("Grounded");
                _stateMachine.TransitionTo(_stateMachine.IdlingState);
            }
            /*else if (CanClimb())
            {
                Debug.Log("Climbing");
                _stateMachine.TransitionTo(_stateMachine.ClimbIdlingState);
            }*/
        }

    }
}
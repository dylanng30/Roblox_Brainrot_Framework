using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Airborne
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.JumpParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.JumpParameterHash);
        }

        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
                        
            StartJumping();
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.FallingState);
        }



        #region ---MAIN METHODS ---
        protected virtual void StartJumping()
        {
            var player =  _stateMachine.PlayerMovement;
            player.RigidBody.AddForce(player.transform.up * _stateMachine.ReusableData.JumpForceModifier,  ForceMode.Impulse);
        }

        #endregion
        
    }
}
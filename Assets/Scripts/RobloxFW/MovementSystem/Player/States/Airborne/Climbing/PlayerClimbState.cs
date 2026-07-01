using RobloxFW.MovementSystem.Player.States.Airborne;
using RobloxFW.MovementSystem.Player;
using RobloxFW.MovementSystem.Utilities;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Airborne.Climbing
{
    public class PlayerClimbState : PlayerAirborneState
    {
        public PlayerClimbState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.PlayerMovement.RigidBody.useGravity = false;
        }

        public override void Exit()
        {
            base.Exit();
            _stateMachine.PlayerMovement.RigidBody.useGravity = true;
        }

        public override void Update()
        {
            base.Update();
            
            if (IsGrounded())
            {
                _stateMachine.TransitionTo(_stateMachine.LandingState);
            }

            /*if (!CanClimb())
            {
                _stateMachine.TransitionTo(_stateMachine.FallingState);
            }*/
            
            ConsumeStamina();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Rotate();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            
            HandleChangeStateInput();
        }

        #region ---MAIN METHODS---

        /*protected override void Rotate()
        {
            var player = _stateMachine.PlayerMovement;
            
            var direction = player.ColliderDetectors.WallDetector.GetDirection();
            direction.y = 0f;
            
            //Debug.DrawRay(playerMovement.transform.position, direction * 2f, Color.red);
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetRotation,
                _movementData.BaseSpeed *  Time.deltaTime
            );
        }*/

        protected virtual void ConsumeStamina()
        {
            //Debug.Log("Stamina consumed");
        }

        #endregion
        
        #region ---HELPER METHODS---
        protected virtual void HandleChangeStateInput()
        {
            
        }

        #endregion
    }
}
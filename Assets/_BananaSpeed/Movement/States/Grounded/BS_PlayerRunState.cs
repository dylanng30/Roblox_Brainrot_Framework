using UnityEngine;

namespace _BananaSpeed.Movement.States.Grounded
{
    public class BS_PlayerRunState : BS_PlayerGroundedState
    {
        public BS_PlayerRunState(BS_PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (_stateMachine.SharedData.MovementInput == Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Vector3 worldDir = GetWorldMoveDirection();
            if (worldDir != Vector3.zero)
            {
                _stateMachine.SharedData.LastMoveDirection = worldDir.normalized;
            }

            UpdateSpeed(_stateMachine.Data.Ground.MaxSpeed);
            ApplyHorizontalVelocity(worldDir.normalized);
            RotateTowardsMoveDirection(worldDir);
        }
    }
}


using UnityEngine;

namespace _BananaSpeed.Movement.States.Airborne
{
    public abstract class BS_PlayerAirborneState : BS_PlayerMovementState
    {
        protected BS_PlayerAirborneState(BS_PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Animation.SetAnimationBool(_stateMachine.Data.AnimationData.IsGroundedHash, false);

            _stateMachine.Input.OnJumpPressed += OnJumpPressed;
        }

        public override void Exit()
        {
            base.Exit();
            _stateMachine.Input.OnJumpPressed -= OnJumpPressed;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            _stateMachine.SharedData.MovementInput = _stateMachine.Input.MovementVector;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            ApplyFallGravity();

            Vector3 worldDir = GetWorldMoveDirection();
            if (worldDir != Vector3.zero)
            {
                ApplyHorizontalVelocity(worldDir.normalized);
                RotateTowardsMoveDirection(worldDir);
            }
        }

        public override void Update()
        {
            base.Update();

            if (_stateMachine.IsGrounded())
            {
                OnLanded();
            }
        }

        protected virtual void OnLanded()
        {
            _stateMachine.Animation.SetAnimationBool(_stateMachine.Data.AnimationData.IsGroundedHash, true);

            if (_stateMachine.SharedData.MovementInput != Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.RunState);
            }
            else
            {
                _stateMachine.TransitionTo(_stateMachine.IdleState);
            }
        }

        protected virtual void OnJumpPressed() { }
    }
}


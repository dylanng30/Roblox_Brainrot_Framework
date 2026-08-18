using UnityEngine;

namespace _BananaSpeed.Movement.States.Grounded
{
    public class BS_PlayerGroundedState : BS_PlayerMovementState
    {
        public BS_PlayerGroundedState(BS_PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _stateMachine.SharedData.JumpsRemaining = 1;
            _stateMachine.SharedData.LastGroundedTime = Time.time;

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

        public override void Update()
        {
            base.Update();

            if (!_stateMachine.IsGrounded())
            {
                _stateMachine.TransitionTo(_stateMachine.FallState);
            }
        }

        protected virtual void OnJumpPressed()
        {
            _stateMachine.TransitionTo(_stateMachine.JumpState);
        }
    }
}

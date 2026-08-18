namespace _BananaSpeed.Movement.States.Grounded
{
    public class BS_PlayerIdleState : BS_PlayerGroundedState
    {
        public BS_PlayerIdleState(BS_PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Animation.SetAnimationBool(_stateMachine.Data.AnimationData.IsGroundedHash, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (_stateMachine.SharedData.MovementInput.sqrMagnitude > 0.01f)
            {
                _stateMachine.TransitionTo(_stateMachine.RunState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            UpdateSpeed(0f);
            ApplyHorizontalVelocity(_stateMachine.SharedData.LastMoveDirection);
        }
    }
}


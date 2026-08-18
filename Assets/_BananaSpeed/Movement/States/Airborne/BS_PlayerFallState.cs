namespace _BananaSpeed.Movement.States.Airborne
{
    public class BS_PlayerFallState : BS_PlayerAirborneState
    {
        public BS_PlayerFallState(BS_PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Animation.StartAnimation(_stateMachine.Data.AnimationData.FallHash);
        }

        protected override void OnJumpPressed()
        {
            if (_stateMachine.SharedData.JumpsRemaining > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.JumpState);
            }
        }
    }
}


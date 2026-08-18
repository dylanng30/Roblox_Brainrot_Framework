using UnityEngine;

namespace _BananaSpeed.Movement.States.Airborne
{
    public class BS_PlayerJumpState : BS_PlayerAirborneState
    {
        public BS_PlayerJumpState(BS_PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _stateMachine.SharedData.JumpsRemaining--;

            ResetVerticalVelocity();

            _stateMachine.Movement.AddImpulse(Vector3.up * _stateMachine.Data.Airborne.JumpForce);

            _stateMachine.Animation.StartAnimation(_stateMachine.Data.AnimationData.JumpHash);
        }

        protected override void OnJumpPressed()
        {
            if (_stateMachine.SharedData.JumpsRemaining > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.SecondJumpState);
            }
        }
    }
}


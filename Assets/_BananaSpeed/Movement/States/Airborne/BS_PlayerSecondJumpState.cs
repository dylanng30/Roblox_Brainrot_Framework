using UnityEngine;

namespace _BananaSpeed.Movement.States.Airborne
{
    public class BS_PlayerSecondJumpState : BS_PlayerAirborneState
    {
        public BS_PlayerSecondJumpState(BS_PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _stateMachine.SharedData.JumpsRemaining = 0;

            ResetVerticalVelocity();

            _stateMachine.Movement.AddImpulse(Vector3.up * _stateMachine.Data.Airborne.SecondJumpForce);

            _stateMachine.Animation.StartAnimation(_stateMachine.Data.AnimationData.SecondJumpHash);
        }

        protected override void OnJumpPressed() { }
    }
}


using RobloxFW.MovementSystem.Player.States;
using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Airborne
{
    public class PlayerAirborneState : PlayerMovementState
    {
        protected PlayerMovementStateMachine _stateMachine;
        
        public PlayerAirborneState(PlayerMovementStateMachine playerStateMachine) : base(playerStateMachine)
        {
            _stateMachine = playerStateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.AirborneParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.AirborneParameterHash);
        }
        
    }
}
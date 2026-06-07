using RobloxFW.MovementSystem.Player.States.Airborne;
using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Airborne.Climbing
{
    public class PlayerClimbingUpState : PlayerAirborneState
    {
        public PlayerClimbingUpState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }
        
        public override void HandleInput()
        {
            
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.ClimbIdlingState);
        }
    }
}
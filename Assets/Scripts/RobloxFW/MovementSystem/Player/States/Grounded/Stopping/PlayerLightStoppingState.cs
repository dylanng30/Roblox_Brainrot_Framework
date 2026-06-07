using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Stopping
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();

            _stateMachine.ReusableData.MovementDecelerationForce = _movementData.StopData.LightDecelerationForce;
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
    }
}
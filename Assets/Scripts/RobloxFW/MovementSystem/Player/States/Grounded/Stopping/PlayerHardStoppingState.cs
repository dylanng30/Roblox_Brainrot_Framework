using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Stopping
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.HardStopParameterHash);
            _stateMachine.ReusableData.MovementDecelerationForce = _movementData.StopData.HardDecelerationForce;
        }

        public override void Exit()
        {
            base.Exit();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.HardStopParameterHash);
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
    }
}
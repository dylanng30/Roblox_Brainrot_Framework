using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Stopping
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }
        public override void Enter()
        {
            base.Enter();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.MediumStopParameterHash);
            _stateMachine.ReusableData.MovementDecelerationForce = _movementData.StopData.MediumDecelerationForce;
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.MediumStopParameterHash);
        }
        
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
    }
}
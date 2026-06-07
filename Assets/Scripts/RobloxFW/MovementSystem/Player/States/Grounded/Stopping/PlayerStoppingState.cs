using RobloxFW.MovementSystem.Player.States.Grounded;
using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Stopping
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.StoppingParameterHash);

            _stateMachine.ReusableData.MovementSpeedModifier = 0f;
        }

        public override void Exit()
        {
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.StoppingParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }
            
            DecelerateHorizontally();
        }

        public override void OnAnimationTransitionEvent()
        {
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
    }
}
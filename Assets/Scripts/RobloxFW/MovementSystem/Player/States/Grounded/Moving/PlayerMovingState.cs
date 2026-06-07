using RobloxFW.MovementSystem.Player.States.Grounded;
using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Moving
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.MovingParameterHash);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.MovingParameterHash);
        }
    }
}
using RobloxFW.MovementSystem.Player.States.Grounded;
using RobloxFW.MovementSystem.Player;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Attacking
{
    public class PlayerAttackingState : PlayerGroundedState
    {
        public PlayerAttackingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.AttackingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.AttackingParameterHash);
        }
    }
}
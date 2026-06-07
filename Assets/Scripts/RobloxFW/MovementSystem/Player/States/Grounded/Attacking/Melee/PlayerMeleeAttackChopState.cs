using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Attacking.Melee
{
    public class PlayerMeleeAttackChopState : PlayerMeleeAttackingState
    {
        public PlayerMeleeAttackChopState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            _stateMachine.ReusableData.AttackForceModifier = 5f;
            Reset();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.MeleeAttackChopParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.MeleeAttackChopParameterHash);
        }
        
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            GoForward();
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
    }
}
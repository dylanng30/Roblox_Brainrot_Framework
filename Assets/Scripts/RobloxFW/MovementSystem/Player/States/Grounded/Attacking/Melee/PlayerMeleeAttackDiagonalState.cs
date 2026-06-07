using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Attacking.Melee
{
    public class PlayerMeleeAttackDiagonalState : PlayerMeleeAttackingState
    {
        public PlayerMeleeAttackDiagonalState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            Reset();
            //StartAnimation(_stateMachine.PlayerMovement.AnimationData.MeleeAttackDiagonalParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            
            //StopAnimation(_stateMachine.PlayerMovement.AnimationData.MeleeAttackDiagonalParameterHash);
        }
        
        public override void Update()
        {
            if (!IsOverTime())
            {
                OnClick();
            }
        }
        
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            
            if (changeNextAttack)
            {
                _stateMachine.TransitionTo(_stateMachine.MeleeStabState);
                return;
            }
            
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
    }
}
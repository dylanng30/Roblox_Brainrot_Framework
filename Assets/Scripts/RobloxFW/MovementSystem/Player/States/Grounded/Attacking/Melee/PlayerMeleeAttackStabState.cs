using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Attacking.Melee
{
    public class PlayerMeleeAttackStabState : PlayerMeleeAttackingState
    {
        public PlayerMeleeAttackStabState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.ReusableData.AttackForceModifier = 2f;
            Reset();
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.MeleeAttackStabParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.MeleeAttackStabParameterHash);
        }
        

        public override void Update()
        {
            if (!IsOverTime())
            {
                OnClick();
            }
        }

        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            GoForward();
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            
            if (changeNextAttack)
            {
                _stateMachine.TransitionTo(_stateMachine.MeleeChopState);
                return;
            }
            
            _stateMachine.TransitionTo(_stateMachine.IdlingState);
        }
        
        
    }
}
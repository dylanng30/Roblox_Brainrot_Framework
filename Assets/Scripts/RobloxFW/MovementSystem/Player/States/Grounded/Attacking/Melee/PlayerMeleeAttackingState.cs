using RobloxFW.MovementSystem.Player.States.Grounded.Attacking;
using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Attacking.Melee
{
    public class PlayerMeleeAttackingState : PlayerAttackingState
    {
        protected bool changeNextAttack;
        protected float startTime;
        private float delayTime = 1f;
        public PlayerMeleeAttackingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            ResetVelocity();
        }

        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            startTime = Time.time;
        }

        public override void PhysicsUpdate()
        {
            
        }
        
        #region ---MAIN METHODS---

        protected virtual void GoForward()
        {
            var player = _stateMachine.PlayerMovement;
            player.RigidBody.AddForce(player.transform.forward * _stateMachine.ReusableData.AttackForceModifier, ForceMode.Impulse);
        }
        #endregion

        #region ---HELPER METHODS---

        protected virtual bool IsOverTime()
        {
            if (Time.time < startTime + delayTime)
            {
                Debug.Log("Can Continue to Attack");
                return false;
            }
            
            return true;
        }
        protected virtual void OnClick()
        {
            if (Input.GetMouseButtonDown(1) && !changeNextAttack)
            {
                changeNextAttack = true;
                Debug.Log("Ready for next attack state");
            }
        }
        protected virtual void Reset()
        {
            changeNextAttack = false;
        }
        #endregion
        
    }
}
using RobloxFW.MovementSystem.Data.Player.States.Grounded;
using RobloxFW.MovementSystem.Player.Components;
using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData  _playerDashData;
        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            _playerDashData = _movementData.DashData;
        }

        public override void Enter()
        {
            base.Enter();
           
            _stateMachine.ReusableData.MovementSpeedModifier = _playerDashData.SpeedModifier;
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.DashParameterHash);

            AddForceOnTransitionFromStationaryState();
            
            DisableDashInput();
            
            //test
            OnAnimationTransitionEvent();

        }

        public override void Exit()
        {
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.DashParameterHash);
        }
        public override void PhysicsUpdate()
        {

        }

        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();

            if (_stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.HardStoppingState);
                return;
            }
            
            _stateMachine.TransitionTo(_stateMachine.SprintingState);
        }

        #region ---MAIN METHODS ---

        private void AddForceOnTransitionFromStationaryState()
        {
            if (_stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            var player = _stateMachine.PlayerMovement;
            Vector3 characterRotationDirection = player.transform.forward;
            characterRotationDirection.y = 0;

            Vector3 force = characterRotationDirection * GetMovementSpeed();
            player.RigidBody.velocity = force;
        }

        private void DisableDashInput()
        {
            _stateMachine.PlayerMovement.PlayerInput.DisableDashAction(_playerDashData.DashLimitReachedCoolDown);
        }

        #endregion
        

    }
}
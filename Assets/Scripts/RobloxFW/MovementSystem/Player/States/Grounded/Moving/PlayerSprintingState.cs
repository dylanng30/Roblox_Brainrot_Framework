using RobloxFW.MovementSystem.Data.Player.States.Grounded;
using RobloxFW.MovementSystem.Player.Components;
using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Moving
{
    public class PlayerSprintingState: PlayerMovingState
    {
        private PlayerSprintData  _sprintData;

        private float startTime;
        private bool keepSprinting = false;
        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            _sprintData = _movementData.SprintData;
        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.SprintParameterHash);
            _stateMachine.ReusableData.MovementSpeedModifier =  _sprintData.SpeedModifier;

            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (keepSprinting)
            {
                return;
            }

            if (Time.time < startTime + _sprintData.SprintToRunTime)
            {
                return;
            }

            StopSprinting();
        }

        public override void Exit()
        {
            base.Exit();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.SprintParameterHash);
            keepSprinting = false;
        }

        #region ---MAIN METHODS---

        private void StopSprinting()
        {
            if (_stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.HardStoppingState);
                return;
            }
            
            _stateMachine.TransitionTo(_stateMachine.RunningState);
        }

        #endregion

        #region ---HELPER METHODS ---

        protected override void HandleChangeStateInput()
        {
            if (_stateMachine.PlayerMovement.PlayerInput.Sprint)
            {
                keepSprinting = true;
            }
        }

        #endregion

    }
}
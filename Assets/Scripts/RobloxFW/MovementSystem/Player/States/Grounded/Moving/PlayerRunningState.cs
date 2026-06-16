using RobloxFW.MovementSystem.Data.Player.States.Grounded;
using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Moving
{
    public class PlayerRunningState : PlayerMovingState
    {
        private PlayerSprintData  _sprintData;
        private float startTime;
        public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            _sprintData = _movementData.SprintData;
        }

        public override void Enter()
        {
            base.Enter();
            
            startTime = Time.time;
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.RunParameterHash);
            _stateMachine.ReusableData.MovementSpeedModifier = _movementData.RunData.SpeedModifier;
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.RunParameterHash);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            HandleChangeStateInput();
        }

        public override void Update()
        {
            base.Update();

            if (Time.time < startTime + _sprintData.RunToWalkTime)
            {
                return;
            }

            StopRunning();
        }
        
        
        #region  ---MAIN METHODS---

        private void StopRunning()
        {
            if (movementInput == Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.IdlingState);
            }
        }
        protected override void HandleChangeStateInput()
        {
            base.HandleChangeStateInput();
            
            /*if (_stateMachine.PlayerMovement.Input.Dash)
            {
                _stateMachine.ChangeState(_stateMachine.DashingState);
            }*/

            if (shouldWalk)
            {
                _stateMachine.TransitionTo(_stateMachine.WalkingState);
            }
        }

        #endregion
        
    }
}
using RobloxFW.MovementSystem.Player.Components;
using RobloxFW.MovementSystem.Player;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.States.Grounded.Moving
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private bool _enteredThisFrame;
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(_stateMachine.PlayerMovement.AnimationData.WalkParameterHash);

            _stateMachine.ReusableData.MovementSpeedModifier = _movementData.WalkData.SpeedModifier;
            
            _enteredThisFrame = true;
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(_stateMachine.PlayerMovement.AnimationData.WalkParameterHash);
        }

        public override void HandleInput()
        {
            base.HandleInput();

            HandleChangeStateInput();
        }

        public override void Update()
        {
            base.Update();
            
            if (_enteredThisFrame)
            {
                _enteredThisFrame = false;
                return;
            }

            StopWalking();
        }
        
        #region  ---MAIN METHODS---
        private void StopWalking()
        {
            if (movementInput == Vector2.zero)
            {
                _stateMachine.TransitionTo(_stateMachine.LightStoppingState);
            }
        }

        protected override void HandleChangeStateInput()
        {
            base.HandleChangeStateInput();
            
            if (_stateMachine.PlayerMovement.PlayerInput.Dash)
            {
                _stateMachine.TransitionTo(_stateMachine.DashingState);
            }
            
            if (!shouldWalk)
            {
                _stateMachine.TransitionTo(_stateMachine.RunningState);
            }
        }

        
        #endregion
    }
}
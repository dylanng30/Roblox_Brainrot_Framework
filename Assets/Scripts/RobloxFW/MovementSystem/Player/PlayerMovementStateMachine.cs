using Dylanng.Core.State;
using RobloxFW.MovementSystem.Data.Player;
using RobloxFW.MovementSystem.Player.Components;
using RobloxFW.MovementSystem.Player.States.Airborne.Climbing.Moving;
using RobloxFW.MovementSystem.Player.States.Airborne.Climbing;
using RobloxFW.MovementSystem.Player.States.Airborne;
using RobloxFW.MovementSystem.Player.States.Grounded.Attacking.Melee;
using RobloxFW.MovementSystem.Player.States.Grounded.Landing;
using RobloxFW.MovementSystem.Player.States.Grounded.Moving;
using RobloxFW.MovementSystem.Player.States.Grounded.Stopping;
using RobloxFW.MovementSystem.Player.States.Grounded;
using RobloxFW.MovementSystem.Utilities;

namespace RobloxFW.MovementSystem.Player
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public PlayerStateReusableData ReusableData { get; private set; }
        public PlayerMovementController PlayerMovement { get; private set; }
        
        public IMovementState CurrentState => _currentState;
        
        // Grounded states
        private PlayerIdlingState _idlingState;
        public PlayerIdlingState IdlingState => _idlingState ??= new PlayerIdlingState(this);
        
        private PlayerDashingState _dashingState;
        public PlayerDashingState DashingState => _dashingState ??= new PlayerDashingState(this);
        
        private PlayerWalkingState _walkingState;
        public PlayerWalkingState WalkingState => _walkingState ??= new PlayerWalkingState(this);
        
        private PlayerRunningState _runningState;
        public PlayerRunningState RunningState => _runningState ??= new PlayerRunningState(this);
        
        private PlayerSprintingState _sprintingState;
        public PlayerSprintingState SprintingState => _sprintingState ??= new PlayerSprintingState(this);

        // Stopping states
        private PlayerLightStoppingState _lightStoppingState;
        public PlayerLightStoppingState LightStoppingState => _lightStoppingState ??= new PlayerLightStoppingState(this);
        
        private PlayerMediumStoppingState _mediumStoppingState;
        public PlayerMediumStoppingState MediumStoppingState => _mediumStoppingState ??= new PlayerMediumStoppingState(this);
        
        private PlayerHardStoppingState _hardStoppingState;
        public PlayerHardStoppingState HardStoppingState => _hardStoppingState ??= new PlayerHardStoppingState(this);
        
        // Airborne states
        private PlayerJumpingState _jumpingState;
        public PlayerJumpingState JumpingState => _jumpingState ??= new PlayerJumpingState(this);
        
        private PlayerFallingState _fallingState;
        public PlayerFallingState FallingState => _fallingState ??= new PlayerFallingState(this);
        
        private PlayerLandingState _landingState;
        public PlayerLandingState LandingState => _landingState ??= new PlayerLandingState(this);
        
        // Melee states
        private PlayerMeleeAttackChopState _meleeChopState;
        public PlayerMeleeAttackChopState MeleeChopState => _meleeChopState ??= new PlayerMeleeAttackChopState(this);
        
        private PlayerMeleeAttackDiagonalState _meleeDiagonalState;
        public PlayerMeleeAttackDiagonalState MeleeDiagonalState => _meleeDiagonalState ??= new PlayerMeleeAttackDiagonalState(this);
        
        private PlayerMeleeAttackStabState _meleeStabState;
        public PlayerMeleeAttackStabState MeleeStabState => _meleeStabState ??= new PlayerMeleeAttackStabState(this);
        
        // Climb states
        private PlayerClimbIdlingState _climbIdlingState;
        public PlayerClimbIdlingState ClimbIdlingState => _climbIdlingState ??= new PlayerClimbIdlingState(this);
        
        private PlayerClimbMovingLeftState _climbMovingLeftState;
        public PlayerClimbMovingLeftState ClimbMovingLeftState => _climbMovingLeftState ??= new PlayerClimbMovingLeftState(this);
        
        private PlayerClimbMovingRightState _climbMovingRightState;
        public PlayerClimbMovingRightState ClimbMovingRightState => _climbMovingRightState ??= new PlayerClimbMovingRightState(this);
        
        private PlayerClimbMovingDownState _climbMovingDownState;
        public PlayerClimbMovingDownState ClimbMovingDownState => _climbMovingDownState ??= new PlayerClimbMovingDownState(this);
        
        private PlayerClimbMovingUpState _climbMovingUpState;
        public PlayerClimbMovingUpState ClimbMovingUpState => _climbMovingUpState ??= new PlayerClimbMovingUpState(this);
        
        private PlayerClimbingUpState _climbingUpState;
        public PlayerClimbingUpState ClimbingUpState => _climbingUpState ??= new PlayerClimbingUpState(this);

        public PlayerMovementStateMachine(PlayerMovementController playerMovement)
        {
            PlayerMovement = playerMovement;
            LoadDatas();
            
            Initialize(IdlingState); 
        }

        #region ---LoadData---

        private void LoadDatas()
        {
            ReusableData = new PlayerStateReusableData();
        }

        #endregion
    }
}
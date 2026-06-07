using UnityEngine;

namespace RobloxFW.MovementSystem.Data.Player
{
    public class PlayerStateReusableData
    {
        public Vector2 MovementInput {get; set;}
        public float MovementSpeedModifier { get; set; } = 1f;
        public float MovementDecelerationForce { get; set; } = 1f;
        public float JumpForceModifier { get; set; } = 10f;
        public bool ShouldWalk {get; set;}

        public float AttackForceModifier { get; set; } = 1f;
        
        private Vector3 currentTargetRotation;
        private Vector3 timeToReachTargetRotation;
        private Vector3 dampedTargetRotationCurrentVelocity;
        private Vector3 dampedTargetRotationPassedTime;

        public ref Vector3 CurrentTargetRotation
        {
            get
            {
                return ref currentTargetRotation;
            }
        }
        public ref Vector3 TimeToReachTargetRotation
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }
        public ref Vector3 DampedTargetRotationCurrentVelocity
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }
        public ref Vector3 DampedTargetRotationPassedTime
        {
            get
            {
                return ref dampedTargetRotationPassedTime;
            }
        }
    }
}
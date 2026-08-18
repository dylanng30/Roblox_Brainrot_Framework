using UnityEngine;

namespace _BananaSpeed.Movement.Data
{
    public class BS_PlayerReusableData
    {
        public Vector2 MovementInput { get; set; }
        
        public Vector3 LastMoveDirection { get; set; }

        public float CurrentSpeed { get; set; }

        public float SpeedSmoothVelocity;

        public float CurrentTargetYAngle { get; set; }

        public float RotationSmoothVelocity;

        public int JumpsRemaining { get; set; }

        public float LastGroundedTime { get; set; }

        public float JumpBufferTimer { get; set; }
    }
}
using System;
using UnityEngine;

namespace _BananaSpeed.Movement.Data
{
    [Serializable]
    public class BS_PlayerAnimationData
    {
        [Header("State Parameter Names")]
        [SerializeField] private string speedParameterName = "Speed";
        [SerializeField] private string isGroundedParameterName = "IsGrounded";
        [SerializeField] private string jumpParameterName = "Jump";
        [SerializeField] private string secondJumpParameterName = "SecondJump";
        [SerializeField] private string fallParameterName = "Fall";

        public int SpeedHash { get; private set; }
        public int IsGroundedHash { get; private set; }
        public int JumpHash { get; private set; }
        public int SecondJumpHash { get; private set; }
        public int FallHash { get; private set; }

        public void Initialize()
        {
            SpeedHash = Animator.StringToHash(speedParameterName);
            IsGroundedHash = Animator.StringToHash(isGroundedParameterName);
            JumpHash = Animator.StringToHash(jumpParameterName);
            SecondJumpHash = Animator.StringToHash(secondJumpParameterName);
            FallHash = Animator.StringToHash(fallParameterName);
        }
    }
}

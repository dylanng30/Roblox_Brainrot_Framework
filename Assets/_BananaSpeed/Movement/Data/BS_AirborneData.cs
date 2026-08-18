using System;
using UnityEngine;

namespace _BananaSpeed.Movement.Data
{
    [Serializable]
    public class BS_AirborneData
    {
        [field: SerializeField] [field: Range(0f, 30f)]
        public float JumpForce { get; private set; } = 9f;

        [field: SerializeField] [field: Range(0f, 30f)]
        public float SecondJumpForce { get; private set; } = 7f;

        [field: SerializeField] [field: Range(1f, 10f)]
        public float FallGravityMultiplier { get; private set; } = 2.8f;

        [field: SerializeField] [field: Range(0f, 0.5f)]
        public float CoyoteTime { get; private set; } = 0.12f;

        [field: SerializeField] [field: Range(0f, 0.5f)]
        public float JumpBufferTime { get; private set; } = 0.1f;
    }
}
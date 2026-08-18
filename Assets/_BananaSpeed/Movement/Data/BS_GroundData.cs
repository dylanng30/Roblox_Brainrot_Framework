using System;
using UnityEngine;

namespace _BananaSpeed.Movement.Data
{
    [Serializable]
    public class BS_GroundData
    {
        [field: SerializeField] [field: Range(0f, 30f)]
        public float MaxSpeed { get; private set; } = 7f;

        [field: SerializeField] [field: Range(0f, 2f)]
        public float AccelerationTime { get; private set; } = 0.25f;

        [field: SerializeField] [field: Range(0f, 2f)]
        public float DecelerationTime { get; private set; } = 0.18f;

        [field: SerializeField] [field: Range(0f, 1f)]
        public float RotationSmoothTime { get; private set; } = 0.12f;
    }
}
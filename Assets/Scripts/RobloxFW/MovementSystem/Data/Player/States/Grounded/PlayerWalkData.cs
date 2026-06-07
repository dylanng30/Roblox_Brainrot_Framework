using System;
using UnityEngine;

namespace RobloxFW.MovementSystem.Data.Player.States.Grounded
{
    [Serializable]
    public class PlayerWalkData
    {
        [field: SerializeField]
        [field: Range(0f, 1f)]
        public float SpeedModifier { get; set; } = 0.225f;
    }
}
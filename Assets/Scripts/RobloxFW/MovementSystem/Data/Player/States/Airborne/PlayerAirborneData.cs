using System;
using UnityEngine;

namespace RobloxFW.MovementSystem.Data.Player.States.Airborne
{
    [Serializable]
    public class PlayerAirborneData
    {
        [field: SerializeField]
        [field: Range(0f, 2f)]
        public float GroundCheckDistance { get; private set; } = 0.1f;
        
        [field: SerializeField]
        [field: Range(0f, 2f)]
        public float WallCheckDistance { get; private set; } = 0.1f;
        
        [field: SerializeField] public PlayerClimbData ClimbData { get; set; }
        
    }
}
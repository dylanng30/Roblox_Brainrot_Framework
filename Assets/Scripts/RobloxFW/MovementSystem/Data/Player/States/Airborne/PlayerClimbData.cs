using System;
using UnityEngine;

namespace RobloxFW.MovementSystem.Data.Player.States.Airborne
{
    [Serializable]
    public class PlayerClimbData
    {
        [field: SerializeField] [field: Range(0f, 5f)] public float WallCheckDistance { get; private set; } = 2f;
        [field: SerializeField] public LayerMask ClimbableLayer { get; private set; } 
    }
}
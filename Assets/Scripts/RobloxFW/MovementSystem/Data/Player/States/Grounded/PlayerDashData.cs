using System;
using UnityEngine;

namespace RobloxFW.MovementSystem.Data.Player.States.Grounded
{
    [Serializable]
    public class PlayerDashData
    {
        [field: SerializeField] [field: Range(1f, 10f)] public float SpeedModifier { get; private set; } = 2f;
        [field: SerializeField] [field: Range(0f, 5f)] public float DashLimitReachedCoolDown { get; private set; } = 1.75f;
    }
}